using System;
using System.ComponentModel;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Superclass of all entities (player, enemy etc.)
	/// All entities require a "Renderer" child to function.
	/// </summary>
	public class Entity : MonoBehaviour
	{
		public Action onDied;

		[HideInInspector] public Healthbar healthbar;
		public float maxHealth;
		public float health;
		public bool usingSafeZone = false;
		public bool invincible = false; // true if in safezone
		public bool dead = false;

		[SerializeField] private GameObject _healthBarPrefab;
		[SerializeField] private float _iFrameSecs;
		private Rigidbody2D _rb;
		private SpriteRenderer _rd;
		private CircleCollider2D _coll2d;
		private float _lastHitTime = Mathf.NegativeInfinity;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_rd = transform.Find("Renderer").GetComponent<SpriteRenderer>();
			_coll2d = GetComponent<CircleCollider2D>();

			healthbar = Instantiate(_healthBarPrefab, transform).GetComponent<Healthbar>();
			healthbar.gameObject.name = "Healthbar";

			onDied += CommonDeathFunc;
		}

		private void Update()
		{
			if (dead || _iFrameSecs == 0) return;

			float timeSinceHit = Time.time - _lastHitTime;
			if (timeSinceHit < _iFrameSecs)
			{
				_rd.enabled = timeSinceHit % 0.1 < 0.05; // blink effect
				invincible = true;
			}
			else
			{
				_rd.enabled = true;
				invincible = usingSafeZone ? true : false;
			}
		}

		public void TryTakeDamage(float damage)
		{
			if (dead || invincible) return;

			health = Mathf.Clamp(health - damage, 0, maxHealth);
			if (health <= 0)
			{
				Debug.Log($"{gameObject.name} has died.");
				onDied.Invoke();
				return;
			}
			
			_lastHitTime = Time.time;
			Debug.Log($"{gameObject.name} down to {health} health.");
		}

		private void CommonDeathFunc()
		{
			dead = true;
			healthbar.Show = false;

			_coll2d.enabled = false;
			_rb.bodyType = RigidbodyType2D.Static;
			_rd.color = new Color(1f, 1f, 1f, 0.075f); // ghost effect
		}
	}

}
