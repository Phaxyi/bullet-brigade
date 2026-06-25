using UnityEngine;

public class Player : MonoBehaviour
{
	[field: SerializeField]
	public float Health { get; private set; }

	[field: SerializeField]
	public bool CanDamage { get; private set; } = true;

	private SpriteRenderer rd;
	private float lastHitTime = 0;

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Debug.Log("im already dead...");
			return;
		}
		
		// TODO: VFX
		lastHitTime = Time.time;
		Debug.Log($"player now at {Health} health.");
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
			CanDamage = false;
		}
		else
		{
			CanDamage = true;
		}
	}
}
