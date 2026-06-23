using UnityEngine;

public class Player : MonoBehaviour
{
	public float health;
	public bool canDamage = true;

	private SpriteRenderer rd;
	private float lastHitTime = 0;

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Debug.Log("im already dead...");
			return;
		}
		
		// TODO: VFX
		lastHitTime = Time.time;
		Debug.Log($"player now at {health} health.");
	}

	private void Awake()
	{
		var renderer = transform.Find("Renderer");
		rd = renderer.GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float timeSinceHit = Time.time - lastHitTime;
		if (timeSinceHit < 0.25)
		{
			rd.enabled = timeSinceHit % 0.1 < 0.05; // blink effect
			canDamage = false;
		}
		else
		{
			canDamage = true;
		}
	}
}
