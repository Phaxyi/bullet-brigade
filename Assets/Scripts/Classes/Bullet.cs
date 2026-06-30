/*
	Handles gun functionality
	Actually firing it is handled in corresponding Entity script (e.g. Player.cs)
*/

using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float _speed;
	private float _damage;
	private float _bouncesLeft;

	private Rigidbody2D _rb;

	// mimic constructor
	public void SetupBullet(float speed, float damage, int maxBounces)
	{
		_speed = speed;
		_damage = damage;
		_bouncesLeft = maxBounces;
	}

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		_rb.linearVelocity = transform.up * _speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// TODO: https://stackoverflow.com/questions/77245272/wall-bounce-in-unity#77245345
		
		GameObject otherObj = collision.gameObject;

		Entity entity = otherObj.GetComponent<Entity>();
		if (entity)
		{
			if (otherObj.CompareTag("Player"))
			{
				// TODO: shoot bullet at yourself to power up?
				return;
			}

			entity.TakeDamage(_damage);
			KillBullet();
			return;
		}

		Wall wall = otherObj.GetComponent<Wall>();
		if (wall)
		{
			Debug.Log(wall);
       		if (_bouncesLeft == 0)
			{
				KillBullet();
				return;
			}
			
			Vector2 normal = collision.GetContact(0).normal;
			_rb.linearVelocity = Vector2.Reflect(_rb.linearVelocity, normal);
			_bouncesLeft -= 1;
			return;
		}
	}

	private void KillBullet() => Destroy(gameObject);
}
