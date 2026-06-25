/*
	Superclass of all entities (player, enemy etc.)
	Errors if entity is missing a "Renderer" child.
*/

using UnityEngine;

public class Entity : MonoBehaviour
{
	[field: SerializeField]
	public float Health { get; private set; }

	[field: SerializeField]
	public bool CanTakeDamage { get; private set; } = true;
	
	[field: SerializeField]
	public bool Dead { get; private set; } = false;

	[SerializeField]
	private float iFrameSecs;

	protected Rigidbody2D rb;
	protected SpriteRenderer rd;
	private float lastHitTime = -1000;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		var renderer = transform.Find("Renderer");
		rd = renderer.GetComponent<SpriteRenderer>();
		if (rd == null)
		{
			Debug.LogError($"{gameObject.name} is without renderer");
		}
	}

	private void Update()
	{
		if (Dead || iFrameSecs == 0) return;

		float timeSinceHit = Time.time - lastHitTime;
		if (timeSinceHit < iFrameSecs)
		{
			rd.enabled = timeSinceHit % 0.1 < 0.05; // blink effect
			CanTakeDamage = false;
		}
		else
		{
			CanTakeDamage = true;
		}
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Dead = true;
			CanTakeDamage = false;
			Debug.Log($"{gameObject.name} has died.");
			return;
		}
		
		lastHitTime = Time.time;
		Debug.Log($"{gameObject.name} down to {Health} health.");
	}
}
