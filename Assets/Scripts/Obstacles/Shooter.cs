using System;
using System.Collections;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Adds shooting behaviour to obstacles to emulate bullet hell (ideally pair with Spinner or some other component).
	/// Optionally uses a string pattern for more complex shooting.
	/// </summary>
	public class Shooter : MonoBehaviour
	{
		[SerializeField] private GameObject _bulletPrefab;
		[SerializeField] private float _shootDelay = 0.2f;
		[SerializeField] private string _shootPattern;

		// bullet fields
		[SerializeField] private float _bulletSpeed = 3;
		[SerializeField] private float _bulletDamage = 10; 
		[SerializeField] private int _maxBounces = 0;

		private Transform _bulletsHolder;
		private Vector2 _spawnOffset;

		private void Awake()
		{
			_bulletsHolder = GameObject.Find("Bullets").transform;
			_spawnOffset = new Vector2(0, 0.5f + _bulletPrefab.transform.lossyScale.y/2 + 0.05f);

			Func<IEnumerator> func = (_shootPattern != default) ? PatternedShootLoop : SimpleShootLoop; 
			StartCoroutine(func());
		}

		private void SpawnBullet()
		{
			GameObject bulletObj = Instantiate(
				_bulletPrefab, transform.TransformPoint(_spawnOffset),
				transform.rotation, _bulletsHolder
			);
			Bullet bullet = bulletObj.GetComponent<Bullet>();
			bullet.SetupBullet(_bulletSpeed, _bulletDamage, _maxBounces, false);
		}

		private IEnumerator SimpleShootLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(_shootDelay);
 				SpawnBullet();
			}
		}

		private IEnumerator PatternedShootLoop()
		{
			int index = 0;
			while (true)
			{
				yield return new WaitForSeconds(_shootDelay);
				index = (index + 1) % _shootPattern.Length;
				if (_shootPattern[index] == '1') SpawnBullet();
			}
		}
	}
}
