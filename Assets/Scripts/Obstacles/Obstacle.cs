using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Superclass for all obstacles aka. Walls, provides basic damage logic.
	/// </summary>
	public class Obstacle : MonoBehaviour
	{
		// used in WallPath
		public float phase = 0; 

		[SerializeField] private bool _instakill;
		[SerializeField] private float _damage;
		private SpriteRenderer _rd;
		private Color _origColor;
		private Color _hitColor;

		private void Awake()
		{
			_rd = GetComponent<SpriteRenderer>();
			_origColor = _rd.color;
			_hitColor = Color.Lerp(_origColor, new Color(1f, 0.5f, 0.5f), 0.5f);
		}

		private void Update()
		{
			float delta = Time.deltaTime;
			_rd.color = Color.Lerp(_rd.color, _origColor, 5 * delta);
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			// functionally useless if no damage to be dealt
			if (!_instakill && _damage <= 0) return;

			Player plr = collision.gameObject.GetComponent<Player>();
			Entity entity = plr ? plr.entity : null;
			if (plr == null || entity.dead || entity.invincible) return;

			entity.TryTakeDamage(_instakill ? float.PositiveInfinity : _damage);
			_rd.color = _hitColor;
		}
	}
}