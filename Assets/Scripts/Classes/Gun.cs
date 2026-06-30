/*
	Handles gun functionality
	Actually firing it is handled in corresponding Entity script (e.g. Player.cs)
*/

using UnityEngine;

namespace BulletBrigade {
	public class Gun : MonoBehaviour
	{
		// accessed by UI
		[field: SerializeField] public float ReloadCooldown { get; private set; } = 1;
		[field: SerializeField] public int MagazineSize { get; private set; } = 8;
		public float LastReloadTime { get; private set; } = Mathf.NegativeInfinity;
		public float BulletsInMag { get; private set; }

		[SerializeField] private GameObject _bulletPrefab;
		[SerializeField] private float _bulletSpeed = 8;
		[SerializeField] private float _bulletDamage = 10; 
		[SerializeField] private float _shootCooldown = 0.15f;
		[SerializeField] private int _maxBounces = 1;
		private float _lastShootTime = Mathf.NegativeInfinity;

		private Transform _bulletsHolder;
		private Transform _offsetObj;

		private void Awake()
		{
			_bulletsHolder = GameObject.Find("Bullets").transform;
			_offsetObj = transform.Find("BulletOffset");

			BulletsInMag = MagazineSize;
		}

		public void Fire()
		{
			if (Time.time - LastReloadTime < ReloadCooldown
				|| Time.time - _lastShootTime < _shootCooldown) return;

			// handle logic
			_lastShootTime = Time.time;
			BulletsInMag -= 1;
			
			if (BulletsInMag == 0)
			{
				LastReloadTime = Time.time;
				BulletsInMag = MagazineSize;
			}

			// shoot
			GameObject bulletObj = Instantiate(
				_bulletPrefab, _offsetObj.position, _offsetObj.rotation, _bulletsHolder
			);

			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.SetupBullet(_bulletSpeed, _bulletDamage, _maxBounces);
		}
	}
}