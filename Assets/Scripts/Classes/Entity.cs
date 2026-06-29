/*
	Superclass of all entities (player, enemy etc.)
	Errors if entity is missing a "Renderer" child.
*/



// TODO: make this component so Player & Enemy inherit from MonoBehaviour so you can do multiple Awakes()

using System;
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

	public float lastHitTime = -float.NegativeInfinity;
	public Rigidbody2D rb;
	public SpriteRenderer rd;
	public Action OnDeadEvent;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

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
			Dead = true;
			CanTakeDamage = false;
			
			OnDeadEvent?.Invoke();
			Debug.Log($"{gameObject.name} has died.");
			return;
		}
		
		lastHitTime = Time.time;
		Debug.Log($"{gameObject.name} down to {Health} health.");
	}
}
