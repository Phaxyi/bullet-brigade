/*
	Moves toward player constantly, slower than the player, when within range.
	Wanders around randomly when outside range.
*/

using UnityEngine;

public class Chaser : MonoBehaviour
{
	[SerializeField]
	private float wanderSpeed;

	[SerializeField]
	private float chaseSpeed;

	[SerializeField]
	private float chaseRange;

	[SerializeField]
	private float rotateSpeed;

	private Rigidbody2D rb;
	private Entity ent;
	private Player plr;
	private Vector2 moveDir = Vector2.zero;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		ent = GetComponent<Entity>();
		plr = FindAnyObjectByType<Player>();
	}

	private void FixedUpdate()
	{
		if (ent.Dead || plr.Dead) return;

		Vector2 dirToPlr = (plr.transform.position - transform.position);
		if (dirToPlr.magnitude < chaseRange) Chase(dirToPlr); else Idle();

		if (moveDir != Vector2.zero)
		{
			AdjustRotation();
		}
	}

	private void Chase(Vector2 dirToPlr)
	{
		moveDir = dirToPlr.normalized;
		rb.linearVelocity = moveDir * chaseSpeed;
	}

	private void Idle()
	{
		moveDir = Vector2.zero;
	}

	private void AdjustRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(transform.forward, moveDir);
		Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

		rb.SetRotation(rotation);
	}
}
