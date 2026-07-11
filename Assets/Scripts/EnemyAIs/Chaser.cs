using System;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Basic "Chaser" enemy that moves toward player constantly, when within range.
	/// Wanders randomly when outside range (idle), also avoids walls in front in this mode.
	/// TODO: visually rotate renderer only
	/// </summary>
	public class Chaser : MonoBehaviour
	{
		[SerializeField] private float _rotateSpeed = 200f;
		[SerializeField] private float _wanderSpeed = 0.8f;
		[SerializeField] private float _chaseSpeed = 2f;
		[SerializeField] private float _chaseRange = 5f;
		[SerializeField] private float _preIdlePause = 1f;

		private const float DETECT_WALL_DIST = 0.1f;
		private Rigidbody2D _rb;
		private Entity _entity;
		private Player _plr;

		private Vector2 _moveDir = Vector2.zero;
		private Vector2 _savedMoveDir;
		private Enemy _enemy;
		private Action<Vector2> _stateFunc;

		private float _seed;
		private float _stateChangeTime = Mathf.NegativeInfinity;
		private float _rotChangeTime = Mathf.NegativeInfinity;
		private int _rotOffset = 0;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_entity = GetComponent<Entity>();
			_enemy = GetComponent<Enemy>();
			_plr = FindAnyObjectByType<Player>();
			_seed = UnityEngine.Random.Range(0f, 100000f);
		}

		private void FixedUpdate()
		{
			if (_entity.dead || _plr.entity.dead) return;

			Vector2 dirToPlr = _plr.transform.position - transform.position;
			if (dirToPlr.magnitude < _chaseRange) ChangeState(Chase);
			else ChangeState(Idle);

			_stateFunc(dirToPlr);
			if (_moveDir != Vector2.zero) AdjustRotation();
		}

		private void ChangeState(Action<Vector2> func)
		{
			if (_stateFunc == func) return;

			_enemy.state = func.Method.Name;
			_stateFunc = func;
			_stateChangeTime = Time.time;
			_savedMoveDir = _moveDir;
		}

		private void Chase(Vector2 dirToPlr)
		{
			_moveDir = dirToPlr.normalized;
			_rb.linearVelocity = _moveDir * _chaseSpeed;
		}

		private void Idle(Vector2 _)
		{
			// short pause before wandering
			float timeSinceStateChange = Time.time - _stateChangeTime;
			if (timeSinceStateChange < _preIdlePause) return;

			// bias _moveDir with rotation when avoiding walls
			if ((Time.time - _rotChangeTime) > 1) {
				Vector2 _frontOffset = new(0, 0.5f);
				RaycastHit2D hit = Physics2D.Raycast(
					transform.TransformPoint(_frontOffset),
					transform.up, DETECT_WALL_DIST, Utils.wallLayerMask);

				Debug.DrawRay(transform.TransformPoint(_frontOffset), transform.up * DETECT_WALL_DIST); // temp
				if (hit.collider) {
					_rotOffset += 120;
					_rotChangeTime = Time.time;
				}
			}

			// use perlin noise to decide movement direction
			float rand = Utils.GetPerlinInterval(_seed, 0.125f);
			_moveDir = Quaternion.Euler(0, 0, _rotOffset)
				* new Vector2(Mathf.Cos(rand), Mathf.Sin(rand)).normalized;

			// lerp _savedMoveDir w/ _moveDir to prevent rotational jerk upon Idle statechange
			float speedAdjust = Mathf.Min(timeSinceStateChange - _preIdlePause, 5) * 1/5;
			_moveDir = Vector2.Lerp(_moveDir, _savedMoveDir, 1 - speedAdjust);
			_rb.linearVelocity = _moveDir * _wanderSpeed * speedAdjust;
		}

		private void AdjustRotation()
		{
			Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _moveDir);
			Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);

			_rb.SetRotation(rotation);
		}
	}
}