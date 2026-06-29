/*
	Superclass of all entities (player, enemy etc.)
	Errors if entity is missing a "Renderer" child.
*/

using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[field: SerializeField]
	public bool CanTakeDamage { get; private set; } = true;
	
	[field: SerializeField]
	public bool Dead { get; private set; } = false;

	[SerializeField]
	private float iFrameSecs;

	[SerializeField]
	private GameObject HealthPrefab;

	private float lastHitTime = Mathf.NegativeInfinity;

	public Rigidbody2D rb;
	public SpriteRenderer rd;
	public Action OnDeadEvent;
	public Healthbar Healthbar;
	public float MaxHealth;
	public float Health;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		Healthbar = Instantiate(HealthPrefab, transform).GetComponent<Healthbar>();
		Healthbar.gameObject.name = "Healthbar";

		Transform renderer = transform.Find("Renderer");
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
			rd.enabled = true;
			CanTakeDamage = true;
		}
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			CommonDeathFunc();
			OnDeadEvent?.Invoke();
			Debug.Log($"{gameObject.name} has died.");
			return;
		}
		
		lastHitTime = Time.time;
		Debug.Log($"{gameObject.name} down to {Health} health.");
	}

	public void CommonDeathFunc()
	{
		Dead = true;
		Healthbar.Show = false;
		CanTakeDamage = false;

		rb.bodyType = RigidbodyType2D.Static;
		rd.color = new Color(1f, 1f, 1f, 0.075f); // "ghost" effect
	}
}
