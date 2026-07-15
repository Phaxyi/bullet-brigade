using System.Collections.Generic;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Handles bullets shot by Gun.
	/// Bullets damage *all* entities, player or enemy alike.
	/// Uses object pooling.
	/// </summary>
	public class Bullet : MonoBehaviour
	{
		private static Transform _bulletsHolder; // static for optimisation
		private static readonly Dictionary<GameObject, List<Bullet>> _inactivePool = new();
		private static readonly Dictionary<GameObject, List<Bullet>> _activePool = new();

		private Rigidbody2D _rb;
		private Vector2 _currentDir;
		private Vector2 _spawnPos;
		private GameObject _prefab;

		private const float DESPAWN_DIST = 20;
		private float _speed;
		private float _damage;
		private int _bouncesLeft;
		private bool _damageEnemies;
		private bool _useDotCollision;

		static Bullet()
		{
			Level.LevelChanged += () =>
			{
				_inactivePool.Clear();
				_activePool.Clear();
				_bulletsHolder = null;
			};
		}

		// mimic constructor structure
		public static void Spawn(GameObject prefab, Transform spawnTrans,
			float speed, float damage, int maxBounces, bool damageEnemies, bool useDotCollision)
		{
			if (!_inactivePool.ContainsKey(prefab))
			{
				_inactivePool.Add(prefab, new());
				_activePool.Add(prefab, new());
			}

			List<Bullet> inactive = _inactivePool[prefab];
			Vector2 spawnPos = spawnTrans.TransformPoint(
				new(0, 0.5f + prefab.transform.lossyScale.y/2 + 0.05f)
			);
			Bullet bullet;

			// retrieve from pool
			if (inactive.Count > 0)
			{
				bullet = inactive[0];
				inactive.RemoveAt(0);

				Transform bulletTrans = bullet.transform;
				bulletTrans.SetPositionAndRotation(spawnPos, spawnTrans.rotation);
				bullet.gameObject.SetActive(true);
			}
			// spawn from scratch
			else 
			{
				GameObject bulletObj = Instantiate(prefab, spawnPos, spawnTrans.rotation, _bulletsHolder);
				bullet = bulletObj.GetComponent<Bullet>();
			}

			bullet._prefab = prefab;
			bullet._speed = speed;
			bullet._damage = damage;
			bullet._bouncesLeft = maxBounces;
			bullet._damageEnemies = damageEnemies;
			bullet._useDotCollision = useDotCollision;

			_activePool[prefab].Add(bullet);
			bullet.Setup();
		}

		private void Awake()
		{
			if (!_bulletsHolder)
				_bulletsHolder = GameObject.Find("/Bullets").transform;
				
			_rb = GetComponent<Rigidbody2D>();
		}

		// function runs on every Spawn() call to re-init changing fields
		private void Setup()
		{
			_currentDir = transform.up;
			_spawnPos = transform.position;
		}

		private void FixedUpdate()
		{
			_rb.linearVelocity = _currentDir * _speed;
			GetRayNormal();

			if (((Vector3)_spawnPos - transform.position).magnitude > DESPAWN_DIST)
			{
				KillBullet();
			}
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject otherObj = collider.gameObject;
			Entity entity = otherObj.GetComponentInParent<Entity>();

			// always damage players; damage enemies if _damageEnemies == true
			bool collideWithPlr = _useDotCollision
				? otherObj.name == "PlayerCentreDot"
				: otherObj.CompareTag("Player");

			if (entity != null)
			{
				if (!(collideWithPlr || _damageEnemies)) return;

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

				_bouncesLeft--;
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

		private void KillBullet() {
			if (!(_activePool.ContainsKey(_prefab)
				&& _activePool[_prefab].Remove(this))) return;

			_inactivePool[_prefab].Add(this);
			gameObject.SetActive(false);
		}
	}	
}
