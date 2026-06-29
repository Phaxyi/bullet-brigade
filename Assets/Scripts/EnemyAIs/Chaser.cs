/*
	Moves toward player constantly, slower than the player, when within range.
	Wanders around randomly when outside range.
*/

using System;
using UnityEngine;

// TODO: raycast walls

public class Chaser : MonoBehaviour
{
	[SerializeField]
	private float wanderSpeed;

	[SerializeField]
	private float chaseSpeed;

	[SerializeField]
	private float chaseRange;

	[SerializeField]
	private float preIdlePause;

	private Rigidbody2D rb;
	private Entity entity;
	private Player plr;

	private Vector2 moveDir = Vector2.zero;
	private float stateChangeTime = Mathf.NegativeInfinity;

	private float perlinX;
	private float perlinY;
	private float perlinStep = 0;
	private Vector2 savedMoveDir;

	private const float rotateSpeed = 200;
	private const float TAU = (float)Math.PI * 2;
	private Action<Vector2> stateFunc;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		entity = GetComponent<Entity>();
		plr = FindAnyObjectByType<Player>();

		perlinX = UnityEngine.Random.Range(0f, 10000f);
		perlinY = UnityEngine.Random.Range(0f, 10000f);
	}

	private void FixedUpdate()
	{
		if (entity.Dead || plr.entity.Dead) return;

		Vector2 dirToPlr = plr.transform.position - transform.position;
		if (dirToPlr.magnitude < chaseRange) ChangeState(Chase);
		else ChangeState(Idle);

		stateFunc(dirToPlr);

		if (moveDir != Vector2.zero)
		{
			AdjustRotation();
		}
	}

	private void ChangeState(Action<Vector2> func)
	{
		if (stateFunc == func) return;

		stateFunc = func;
		stateChangeTime = Time.time;
		savedMoveDir = moveDir;
	}

	private void Chase(Vector2 dirToPlr)
	{
		moveDir = dirToPlr.normalized;
		rb.linearVelocity = moveDir * chaseSpeed;
	}

	private void Idle(Vector2 _)
	{
		moveDir = Vector2.zero;

		// short pause before wandering
		float timeSinceStateChange = Time.time - stateChangeTime;
		if (timeSinceStateChange < preIdlePause) return;

		// use perlin noise to decide movement
		// lerp savedMoveDir w/ moveDir to prevent rotational jerk upon Idle statechange
		float speedAdjust = Mathf.Min(timeSinceStateChange - preIdlePause, 5) * 1/5;
		perlinStep += Time.fixedDeltaTime / 5;
		
		float rand = Mathf.Clamp(Mathf.PerlinNoise(
			perlinX + perlinStep,
			perlinY - perlinStep
		), 0, 1) * TAU;

		moveDir = new Vector2(Mathf.Cos(rand), Mathf.Sin(rand)).normalized;
		moveDir = Vector2.Lerp(moveDir, savedMoveDir, 1 - speedAdjust);
		rb.linearVelocity = moveDir * wanderSpeed * speedAdjust;
	}

	private void AdjustRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(transform.forward, moveDir);
		Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

		rb.SetRotation(rotation);
	}
}
