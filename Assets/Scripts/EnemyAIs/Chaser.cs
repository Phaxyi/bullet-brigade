using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace BulletBrigade
{
	/// <summary>
	/// Basic "Chaser" enemy that moves toward player constantly, when within range.
	/// Wanders randomly when outside range (idle)
	/// Avoids walls in front of it.
	/// </summary>
	public class Chaser : MonoBehaviour
	{
		private const float TAU = (float)Math.PI * 2;
		
		[SerializeField] private float _rotateSpeed = 200f;
		[SerializeField] private float _wanderSpeed = 1f;
		[SerializeField] private float _chaseSpeed = 2f;
		[SerializeField] private float _chaseRange = 5f;
		[SerializeField] private float _preIdlePause = 1f;

		private Rigidbody2D _rb;
		private Entity _entity;
		private Player _plr;
		private LayerMask _layerMask;

		private Vector2 _moveDir = Vector2.zero;
		private Vector2 _savedMoveDir;
		private Action<Vector2> _stateFunc;
		private float _stateChangeTime = Mathf.NegativeInfinity;

		private float _perlinX;
		private float _perlinY;
		private float _perlinStep = 0;
		private int _rotDegreeOffset = 0;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_entity = GetComponent<Entity>();
			_plr = FindAnyObjectByType<Player>();
			_layerMask = LayerMask.GetMask("Wall");

			_perlinX = UnityEngine.Random.Range(0f, 10000f);
			_perlinY = UnityEngine.Random.Range(0f, 10000f);
		}

		private void FixedUpdate()
		{
			if (_entity.dead || _plr.entity.dead) return;

			Vector2 dirToPlr = _plr.transform.position - transform.position;
			if (dirToPlr.magnitude < _chaseRange) ChangeState(Chase);
			else ChangeState(Idle);

			_stateFunc(dirToPlr);

			if (_moveDir != Vector2.zero)
			{
				AdjustRotation();
			}
		}

		private void ChangeState(Action<Vector2> func)
		{
			if (_stateFunc == func) return;

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

			// use perlin noise to decide movement direction
			float speedAdjust = Mathf.Min(timeSinceStateChange - _preIdlePause, 5) * 1/5;
			_perlinStep += Time.fixedDeltaTime / 5;

			float rand = Mathf.Clamp(Mathf.PerlinNoise(
				_perlinX + _perlinStep,
				_perlinY - _perlinStep
			), 0, 1) * TAU;

			// bias `_moveDir` with rotation when avoiding walls
			const float DETECT_WALL_DIST = 0.25f;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDir, DETECT_WALL_DIST, _layerMask);
			if (hit) _rotDegreeOffset += 160;

			_moveDir = Quaternion.Euler(0, 0, _rotDegreeOffset)
				* new Vector2(Mathf.Cos(rand), Mathf.Sin(rand)).normalized;

			// lerp savedMoveDir w/ moveDir to prevent rotational jerk upon Idle statechange
			_moveDir = Vector2.Lerp(_moveDir, _savedMoveDir, 1 - speedAdjust);
			_rb.linearVelocity = _moveDir * _wanderSpeed * speedAdjust;
		}

		private void AdjustRotation()
		{
			Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _moveDir);
			Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);

			_rb.SetRotation(targetRotation);
		}
	}
}