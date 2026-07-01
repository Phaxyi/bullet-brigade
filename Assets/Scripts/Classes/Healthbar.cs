using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Discreet visualisation of Entity health, placed above it.
	/// </summary>
	public class Healthbar : MonoBehaviour
	{
		public bool Show = true;

		private SpriteRenderer _rd;
		private Transform _parent;
		private Entity _entity;

		private void Awake()
		{
			_rd = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			_parent = transform.parent;
			_entity = _parent.GetComponent<Entity>();

			Utils.SetGlobalScale(transform, Vector2.one * 1.75f);
		}

	private void Update()
		{
			_rd.enabled = Show;
			if (!Show) return;

			_rd.color = new Color(1, 1, 1, _entity.health / _entity.maxHealth);
			
			// anchor healthbar above parent
			transform.position = new Vector2(
				_parent.position.x,
				_parent.position.y + _parent.lossyScale.y/2 + 0.4f
			);
		}
	}
}