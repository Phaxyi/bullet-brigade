using UnityEngine;

namespace BulletBrigade {
	public class Safezone : MonoBehaviour
	{
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
			plr.entity.usingSafeZone = true;
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player")) return;

			Player plr = collision.GetComponent<Player>();
			plr.entity.usingSafeZone = false;
		}
	}
}
