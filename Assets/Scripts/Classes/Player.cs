using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
	[SerializeField]
	private float moveSpeed;

	private const float rotateSpeed = 200;
	private Vector2 moveDir = Vector2.zero;

    public void FixedUpdate()
    {
		if (Dead) return;
    	rb.linearVelocity = moveDir * moveSpeed;

		if (moveDir != Vector2.zero)
		{
			AdjustRotation();
		}
    }

	public void OnMove(InputValue inputVal)
	{
		moveDir = inputVal.Get<Vector2>();
	}

	private void AdjustRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(transform.forward, moveDir);
		Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

		rb.SetRotation(rotation);
	}
}
