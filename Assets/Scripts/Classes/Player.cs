using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	// make public for convenience
	[HideInInspector] public Entity entity;

	private const float ROTATE_SPEED = 200;

	[SerializeField] private float _moveSpeed = 4f;
	private Vector2 _moveDir = Vector2.zero;
	private Rigidbody2D _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		entity = GetComponent<Entity>();
		entity.onDeadEvent = OnDead;
	}

    public void FixedUpdate()
    {
		if (entity.dead) return;
    	_rb.linearVelocity = _moveDir * _moveSpeed;

		if (_moveDir != Vector2.zero)
		{
			AdjustRotation();
		}
    }

	public void OnMove(InputValue inputVal)
	{
		_moveDir = inputVal.Get<Vector2>();
	}

	private void AdjustRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _moveDir);
		Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, ROTATE_SPEED * Time.fixedDeltaTime);

		_rb.SetRotation(rotation);
	}

	private void OnDead()
	{
		Debug.Log("im dead (Player.cs)");
	}
}
