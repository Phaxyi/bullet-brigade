/*
	Handles gun functionality
	Actually firing it is handled in corresponding Entity script (e.g. Player.cs)
*/

using UnityEngine;

namespace BulletBrigade {
	public class Gun : MonoBehaviour
	{
		[SerializeField] private GameObject _bulletPrefab;

		[SerializeField] private float _bulletSpeed = 8;
		[SerializeField] private float _bulletDamage = 10; 
		[SerializeField] private float _reloadCooldown = 1;
		[SerializeField] private float _shootCooldown = 0.15f;
		[SerializeField] private int _magazineSize = 8;
		[SerializeField] private int _maxBounces = 1;

		private Transform _bulletsHolder;
		private Transform _offsetObj;

		private void Awake()
		{
			_bulletsHolder = GameObject.Find("Bullets").transform;
			_offsetObj = transform.Find("BulletOffset");
		} 

		public void Fire()
		{
			GameObject bulletObj = Instantiate(
				_bulletPrefab, _offsetObj.position, _offsetObj.rotation, _bulletsHolder
			);

			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.SetupBullet(_bulletSpeed, _bulletDamage, _maxBounces);
		}
	}
}