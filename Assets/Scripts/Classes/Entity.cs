/*
	Superclass of all entities (player, enemy etc.)
	Errors if entity is missing a "Renderer" child.
*/

using System;
using UnityEngine;

namespace BulletBrigade
{
	public class Entity : MonoBehaviour
	{
		public Action onDied;

		[HideInInspector] public Healthbar healthbar;
		public float maxHealth;
		public float health;
		public bool canTakeDamage = true;
		public bool dead = false;

		[SerializeField] private GameObject _healthBarPrefab;
		[SerializeField] private float _iFrameSecs;
		private Rigidbody2D _rb;
		private SpriteRenderer _rd;
		private float _lastHitTime = Mathf.NegativeInfinity;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_rd = transform.Find("Renderer").GetComponent<SpriteRenderer>();

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
				canTakeDamage = false;
			}
			else
			{
				_rd.enabled = true;
				canTakeDamage = true;
			}
		}

		public void TakeDamage(float damage)
		{
			health -= damage;
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
			canTakeDamage = false;

			_rb.bodyType = RigidbodyType2D.Static;
			_rd.color = new Color(1f, 1f, 1f, 0.075f); // ghost effect
		}
	}

}
