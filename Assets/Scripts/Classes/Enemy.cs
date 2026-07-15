using System;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Enemy superclass handing common functions.
	/// </summary>
	public class Enemy : MonoBehaviour
	{
		[HideInInspector] public string state;
		public static Action EnemyDied;
		
		[SerializeField] private float _damage;
		private Entity _entity;

		private void Awake()
		{
			_entity = GetComponent<Entity>();
			_entity.onDied += OnDied;
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			if (_entity.dead) return;

			Player plr = collision.gameObject.GetComponent<Player>();
			Entity entity = plr ? plr.entity : null;
			
			if (plr != null) entity.TryTakeDamage(_damage);
		}

		private void OnDied()
		{
			EnemyDied?.Invoke();
		}
	}
}