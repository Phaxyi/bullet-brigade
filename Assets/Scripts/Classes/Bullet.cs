using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Handles bullets shot by Gun.
	/// Bullets damage *all* entities, player or enemy alike.
	/// </summary>
	public class Bullet : MonoBehaviour
	{
		private float _speed;
		private float _damage;
		private float _bouncesLeft;
		private bool _damageEnemies;

		private Rigidbody2D _rb;
		private Vector2 _currentDir;

		// mimic constructor structure
		public void SetupBullet(float speed, float damage, int maxBounces, bool damageEnemies)
		{
			_speed = speed;
			_damage = damage;
			_bouncesLeft = maxBounces;
			_damageEnemies = damageEnemies;
		}

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_currentDir = transform.up;
		}

		private void FixedUpdate()
		{
			_rb.linearVelocity = _currentDir * _speed;
			GetRayNormal();
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject otherObj = collider.gameObject;
			Entity entity = otherObj.GetComponent<Entity>();

			// always damage players; damage enemies if _damageEnemies == true
			if (entity != null)
			{
				if (otherObj.tag != "Player" && !_damageEnemies) return;

				entity.TryTakeDamage(_damage);
				KillBullet();
				return;
			}
			Debug.Log("finding wall");
			Obstacle wall = otherObj.GetComponent<Obstacle>();
			if (wall != null)
			{
				if (_bouncesLeft == 0)
				{
					KillBullet();
					return;
				}
				
				// raycast because you can't get contacts
				// from OnTrigger & can't ignore collision with OnCollision..?
				Vector2 dir = Vector2.Reflect(_rb.linearVelocity.normalized, GetRayNormal());
				_rb.SetRotation(Quaternion.LookRotation(transform.forward, dir));
				_currentDir = dir;

				_bouncesLeft -= 1;
				return;
			}
		}

		private Vector2 GetRayNormal()
		{
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position - (transform.up * 0.2f),
				transform.up,
				Mathf.Infinity,
				Utils.wallLayerMask
			);
			return hit.normal;
		}

		private void KillBullet() => Destroy(gameObject);
	}	
}
