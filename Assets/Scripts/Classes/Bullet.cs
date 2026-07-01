/*
	Handles gun functionality
	Actually firing it is handled in corresponding Entity script (e.g. Player.cs)
	Damages *all* entities, player or enemy
*/

using UnityEngine;

namespace BulletBrigade
{
	public class Bullet : MonoBehaviour
	{
		private float _speed;
		private float _damage;
		private float _bouncesLeft;

		private Rigidbody2D _rb;
		private LayerMask _layerMask;
		private Vector2 _currentDir;
		private Vector2 _rayNormal;

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
			_layerMask = LayerMask.GetMask("Wall");

			_currentDir = transform.up;
		}

		private void FixedUpdate()
		{
			_rb.linearVelocity = _currentDir * _speed;
			UpdateRayNormal();
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject otherObj = collider.gameObject;

			Entity entity = otherObj.GetComponent<Entity>();
			if (entity)
			{
				// if (otherObj.CompareTag("Player"))
				entity.TakeDamage(_damage);
				KillBullet();
				return;
			}

			Obstacle wall = otherObj.GetComponent<Obstacle>();
			if (wall)
			{
				if (_bouncesLeft == 0)
				{
					KillBullet();
					return;
				}
				
				// use Raycast because you can't get contacts
				// from OnTrigger & can't ignore collision with OnCollision..?
				Vector2 dir = Vector2.Reflect(_rb.linearVelocity.normalized, _rayNormal);
				_rb.SetRotation(Quaternion.LookRotation(transform.forward, dir));
				_currentDir = dir;

				_bouncesLeft -= 1;
				return;
			}
		}

		private void UpdateRayNormal()
		{
			RaycastHit2D _hit = Physics2D.Raycast(
				transform.position - (transform.up * transform.lossyScale.y),
				transform.TransformDirection(Vector2.up),
				Mathf.Infinity,
				_layerMask
			);
			_rayNormal = _hit.normal;
		}

		private void KillBullet() => Destroy(gameObject);
	}	
}
