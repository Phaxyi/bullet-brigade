using UnityEngine;

public class Enemy : MonoBehaviour
{
	[field: SerializeField]
	public float Damage { get; private set; }
	private Entity entity;

	private void Awake()
	{
		entity = GetComponent<Entity>();
		entity.OnDeadEvent = OnDead;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (entity.Dead) return;

		Player plr = collision.gameObject.GetComponent<Player>();
		if (plr == null || !plr.entity.CanTakeDamage) return;

		plr.entity.TakeDamage(Damage);
	}

	private void OnDead()
	{
		
	}
}