using System;
using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Safe zone to make players invincible. Enemy AIs avoid walking into it (ideally).
	/// </summary>
	public class Safezone : MonoBehaviour
	{
		public static Action TouchedExitZone;
		public bool _isExitZone;

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

			// special VFX for exit zone
			if (!_isExitZone) return;
			GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.3f, 0.3f);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player")) return;

			Player plr = collision.GetComponent<Player>();
			plr.usingSafeZone = true;

			if (_isExitZone) TouchedExitZone?.Invoke();
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player")) return;

			Player plr = collision.GetComponent<Player>();
			plr.usingSafeZone = false;
		}
	}
}
