using UnityEngine;

public class Safe : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Player plr = collision.gameObject.GetComponent<Player>();
		if (plr == null) return;

		Level.CollectSafe();
		Destroy(gameObject);
	}
}
