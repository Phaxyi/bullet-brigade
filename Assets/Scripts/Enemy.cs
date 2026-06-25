using UnityEngine;

public class Enemy : Entity
{
	[field: SerializeField]
	public float Damage { get; private set; }

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (Dead) return;

		Player plr = collision.gameObject.GetComponent<Player>();
		if (plr == null || !plr.CanTakeDamage) return;

		plr.TakeDamage(Damage);
	}
}