using System;
using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Safe zone to make players invincible. Enemy AIs avoid walking into it (ideally).
	/// </summary>
	public class Safezone : MonoBehaviour
	{
		public static Action EnteredSafezone; // TODO: make onlyy a certain safezone able to move to next level

		private void Awake()
		{
			Player plr = FindAnyObjectByType<Player>();
			BoxCollider2D coll2D = GetComponent<BoxCollider2D>();

			// scale inwards, so player must be half-inside to activate safezone
			Vector2 lossy = transform.lossyScale;
			coll2D.size = new Vector2(
				(lossy.x - plr.transform.lossyScale.x) / lossy.x,
				(lossy.y - plr.transform.lossyScale.y) / lossy.y
			);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player")) return;

			Player plr = collision.GetComponent<Player>();
			plr.usingSafeZone = true;
			EnteredSafezone?.Invoke();
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player")) return;

			Player plr = collision.GetComponent<Player>();
			plr.usingSafeZone = false;
		}
	}
}
