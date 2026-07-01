using UnityEngine;

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

			// shoot without touching shooter (y/2 doesn't work?)
			GameObject bulletObj = Instantiate(
				_bulletPrefab,
				_offsetObj.position + _offsetObj.up * _bulletPrefab.transform.lossyScale.y,
				_offsetObj.rotation,
				_bulletsHolder
			);

			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.SetupBullet(_bulletSpeed, _bulletDamage, _maxBounces);
		}
	}
}