/*
	Handles gun functionality
	Actually firing it is handled in corresponding Entity script (e.g. Player.cs)
*/

using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField] private GameObject _bulletPrefab;

	[SerializeField] private float _bulletSpeed = 999;
	[SerializeField] private float _magazineSize;
	[SerializeField] private float _reloadCooldown;
	[SerializeField] private float _shootCooldown;

	public void Fire()
	{
		
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		// TODO: https://stackoverflow.com/questions/77245272/wall-bounce-in-unity#77245345
	}
}
