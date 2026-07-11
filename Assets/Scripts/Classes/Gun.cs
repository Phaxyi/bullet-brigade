using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletBrigade {
	/// <summary>
	/// Handles gun functionality.
	/// Actual firing is done in corresponding Entity (e.g. Player). Enemies can wield guns too.
	/// </summary>
	public class Gun : MonoBehaviour
	{
		// accessed by UI
		[field: SerializeField] public float ReloadCooldown { get; private set; } = 1;
		[field: SerializeField] public int MagazineSize { get; private set; } = 8;
		public float LastReloadTime { get; private set; } = Mathf.NegativeInfinity;
		public float BulletsInMag { get; private set; }

		// gun controller
		private static readonly List<Gun> _inputBoundGuns = new();
		[SerializeField] private bool _manualControl;
		[SerializeField] private bool _damageEnemies;
		[SerializeField] private float _autoShootDelay;

		// bullet fields
		[SerializeField] private GameObject _bulletPrefab;
		[SerializeField] private float _shootCooldown = 0.15f;
		[SerializeField] private float _bulletSpeed = 8;
		[SerializeField] private float _bulletDamage = 10; 
		[SerializeField] private int _maxBounces = 1;

		private Entity _entity;
		private Enemy _enemy;
		private Transform _bulletsHolder;
		private Vector2 _spawnOffset;
		private float _lastShootTime = Mathf.NegativeInfinity;

		private void Awake()
		{
			_entity = gameObject.GetComponent<Entity>();
			_enemy = gameObject.GetComponent<Enemy>();
			_bulletsHolder = GameObject.Find("Bullets").transform;
			// spawn without touching entity itself
			_spawnOffset = new Vector2(0, 0.5f + _bulletPrefab.transform.lossyScale.y/2 + 0.05f);

			BulletsInMag = MagazineSize;
			if (_manualControl) _inputBoundGuns.Add(this);
			else StartCoroutine(AutoShoot());
		}

		private IEnumerator AutoShoot()
		{
			while (true && !_entity.dead)
			{
				// admittedly checking state like this is quite dangerous since
				// you'd have to ensure all chase states are named "Chase"
				yield return new WaitForSeconds(_autoShootDelay);
				if (_enemy.state != "Chase") continue;

				TryShoot();
			}
		}

		public void TryShoot()
		{
			if (_entity.dead || _entity.invincible
				|| Time.time - LastReloadTime < ReloadCooldown
				|| Time.time - _lastShootTime < _shootCooldown) return;

			// don't shoot if too close to wall
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position, transform.TransformDirection(Vector2.up), 0.25f, Utils.wallLayerMask);
			if (hit.collider != null) return;

			// handle shoot logic
			_lastShootTime = Time.time;
			BulletsInMag -= 1;

			if (BulletsInMag == 0)
			{
				LastReloadTime = Time.time;
				BulletsInMag = MagazineSize;
			}

			GameObject bulletObj = Instantiate(
				_bulletPrefab, transform.TransformPoint(_spawnOffset),
				transform.rotation, _bulletsHolder
			);
			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.SetupBullet(_bulletSpeed, _bulletDamage, _maxBounces, _damageEnemies, false);
		}

		private void OnFire(InputValue _)
		{
			foreach (Gun manualGun in _inputBoundGuns)
			{
				manualGun.TryShoot();
			}
		}
	}
}