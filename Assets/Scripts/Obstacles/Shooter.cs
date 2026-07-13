using System;
using System.Collections;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Adds shooting behaviour to obstacles to emulate bullet hell, ideally paired with Spinner or some other component.
	/// Optionally uses a string pattern for more complex shooting.
	/// Uses the player's centre for collision (closer behaviour to Touhou)
	/// </summary>
	public class Shooter : MonoBehaviour
	{
		[SerializeField] private GameObject _bulletPrefab;
		[SerializeField] private float _shootDelay = 0.2f;
		[SerializeField] private string _shootPattern = ""; // a string where '1' = shoot, '0' = rest.
		 													// each character waits for _shootDelay s.
		[SerializeField] private float _bulletSpeed = 3;
		[SerializeField] private float _bulletDamage = 10; 
		[SerializeField] private int _maxBounces = 0;

		private void Awake()
		{
			Func<IEnumerator> func = (_shootPattern != "") ? PatternedShootLoop : SimpleShootLoop; 
			StartCoroutine(func());
		}

		private void CallBulletSpawn()
			=> Bullet.Spawn(_bulletPrefab, transform, _bulletSpeed, _bulletDamage, _maxBounces, false, true);

		private IEnumerator SimpleShootLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(_shootDelay);
 				CallBulletSpawn();
			}
		}

		private IEnumerator PatternedShootLoop()
		{
			int index = 0;
			while (true)
			{
				yield return new WaitForSeconds(_shootDelay);
				index = (index + 1) % _shootPattern.Length;

				if (_shootPattern[index] == '1') CallBulletSpawn();
			}
		}
	}
}
