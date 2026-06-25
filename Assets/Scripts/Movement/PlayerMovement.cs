using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float rotateSpeed;

	private Rigidbody2D rb;
	private Vector2 moveDir = Vector2.zero;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {		
    	rb.linearVelocity = moveDir * moveSpeed;

		if (moveDir != Vector2.zero)
		{	
			Quaternion targetRotation = Quaternion.LookRotation(transform.forward, moveDir);
			Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

			rb.SetRotation(rotation);
		}
    }

	public void OnMove(InputValue inputVal)
	{
		moveDir = inputVal.Get<Vector2>();
	}
}
