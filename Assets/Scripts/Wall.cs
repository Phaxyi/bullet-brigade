using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public bool instakill;
	public float damage;

	private SpriteRenderer rd;

	private void Awake()
	{
		rd = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float delta = Time.deltaTime;
		rd.color = Color.Lerp(rd.color, Color.white, 5 * delta);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		var plr = collision.gameObject.GetComponent<Player>();
		if (plr == null || !plr.canDamage) return;

		plr.TakeDamage(instakill ? float.PositiveInfinity : damage);
		rd.color = new Color(1f, 0.5f, 0.5f);
	}
}
