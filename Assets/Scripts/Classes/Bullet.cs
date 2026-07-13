using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Handles bullets shot by Gun.
	/// Bullets damage *all* entities, player or enemy alike.
	/// </summary>
	public class Bullet : MonoBehaviour
	{
		private const float DESPAWN_DIST = 20;
		private float _speed;
		private float _damage;
		private int _bouncesLeft;
		private bool _damageEnemies;
		private bool _useDotCollision;

		private Rigidbody2D _rb;
		private Vector2 _currentDir;
		private Vector3 _spawnPos;

		// mimic constructor structure
		public void SetupBullet(float speed, float damage,
			int maxBounces, bool damageEnemies, bool useDotCollision)
		{
			_speed = speed;
			_damage = damage;
			_bouncesLeft = maxBounces;
			_damageEnemies = damageEnemies;
			_useDotCollision = useDotCollision;
		}

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_currentDir = transform.up;
			_spawnPos = transform.position;
		}

		private void FixedUpdate()
		{
			_rb.linearVelocity = _currentDir * _speed;
			GetRayNormal();

			if ((_spawnPos - transform.position).magnitude > DESPAWN_DIST)
			{
				KillBullet();
			}
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject otherObj = collider.gameObject;
			Entity entity = otherObj.GetComponentInParent<Entity>();

			// always damage players; damage enemies if _damageEnemies == true
			bool collidewPlr = _useDotCollision
				? otherObj.name == "PlayerCentreDot"
				: otherObj.CompareTag("Player");

			if (entity != null)
			{
				if (!(collidewPlr || _damageEnemies)) return;

				entity.TryTakeDamage(_damage);
				KillBullet();
				return;
			}

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
