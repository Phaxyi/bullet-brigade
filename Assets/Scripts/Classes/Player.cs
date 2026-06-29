using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	private const float rotateSpeed = 200;
	private Vector2 moveDir = Vector2.zero;
	public Entity entity;

	private void Awake()
	{
		entity = GetComponent<Entity>();
		entity.OnDeadEvent = OnDead;
	}

    public void FixedUpdate()
    {
		if (entity.Dead) return;
    	entity.rb.linearVelocity = moveDir * moveSpeed;

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

		entity.rb.SetRotation(rotation);
	}

	private void OnDead()
	{
		Debug.Log("im dead fr fr");
	}
}
