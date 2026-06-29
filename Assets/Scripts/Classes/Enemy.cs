using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float _damage;
	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
		// entity.OnDeadEvent = OnDead;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (_entity.dead) return;

		Player plr = collision.gameObject.GetComponent<Player>();
		if (plr == null || !plr.entity.canTakeDamage) return;

		plr.entity.TakeDamage(_damage);
	}
}