using UnityEngine;

public class Wall : MonoBehaviour
{
	[field: SerializeField]
	public bool Instakill { get; private set; }

	[field: SerializeField]
	public float Damage { get; private set; }

	private SpriteRenderer rd;
	private Color origColor;
	private Color hitColor;

	private void Awake()
	{
		rd = GetComponent<SpriteRenderer>();
		origColor = rd.color;
		hitColor = Color.Lerp(origColor, new Color(1f, 0.5f, 0.5f), 0.5f); // ?
	}

	private void Update()
	{
		float delta = Time.deltaTime;
		rd.color = Color.Lerp(rd.color, origColor, 5 * delta);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		// functionally useless if no damage to be dealt
		if (!Instakill && Damage <= 0) return;

		var plr = collision.gameObject.GetComponent<Player>();
		if (plr == null || !plr.CanDamage) return;

		plr.TakeDamage(Instakill ? float.PositiveInfinity : Damage);
		rd.color = hitColor;
	}
}
