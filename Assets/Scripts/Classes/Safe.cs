using System;
using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Collectibles used for completing level.
	/// </summary>
	public class Safe : MonoBehaviour
	{
		public static Action SafeCollected;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Player plr = collision.gameObject.GetComponent<Player>();
			if (plr == null) return;

			SafeCollected?.Invoke();
			Destroy(gameObject);
		}
	}
}