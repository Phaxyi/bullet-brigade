using UnityEngine;

public class Enemy : MonoBehaviour
{
	[field: SerializeField]
	public float Health { get; private set; }

	[field: SerializeField]
	public float Damage { get; private set; }

	public void TakeDamage(float damage)
	{
		// enemy should ideally lack damage cooldown
		Health -= damage;
		if (Health <= 0)
		{
			Destroy(gameObject);
			Debug.Log($"enemy died.");
			return;
		}
		
		Debug.Log($"enemy now at {Health} health.");
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		var plr = collision.gameObject.GetComponent<Player>();
		if (plr == null || !plr.CanDamage) return;

		plr.TakeDamage(Damage);
	}
}
