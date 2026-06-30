using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	// make public for convenience
	[HideInInspector] public Entity entity;

	[SerializeField] private float _rotateSpeed = 275f;
	[SerializeField] private float _moveSpeed = 4f;
	private Vector2 _moveDir = Vector2.zero;
	private Rigidbody2D _rb;
	private Gun _gun;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_gun = GetComponent<Gun>();

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

	private void AdjustRotation()
	{
		Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _moveDir);
		Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);

		_rb.SetRotation(rotation);
	}

	private void OnDead()
	{
		Debug.Log("im dead (Player.cs)");
	}

	// INPUT
	private void OnMove(InputValue inputVal)
	{
		_moveDir = inputVal.Get<Vector2>();
	}

	private void OnFire(InputValue inputVal) => _gun.Fire();
}
