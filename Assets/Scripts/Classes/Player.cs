using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletBrigade {
	/// <summary>
	/// Handles player-centric logic and movement.
	/// Press E to dash.
	/// </summary>
	public class Player : MonoBehaviour
	{
		[field: SerializeField] public float DashCooldown { get; private set; } = 3f;
		[field: SerializeField] public float LastDashTime { get; private set; } = Mathf.NegativeInfinity;
		[HideInInspector] public Entity entity;
		[HideInInspector] public bool usingSafeZone;

		[SerializeField] private float _rotateSpeed = 275f;
		[SerializeField] private float _moveSpeed = 4f;
		[SerializeField] private float _dashDistance = 2f;

		private Transform _persistent;
		private Transform _renderer;
		private Vector2 _moveDir = Vector2.zero;
		private Rigidbody2D _rb;

		private void Awake()
		{
			entity = GetComponent<Entity>();
			entity.onDied += OnDied;

			_persistent = GameObject.Find("Persistent").transform;
			_renderer = transform.Find("Renderer");
			_rb = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			if (entity.dead) return;
			_rb.linearVelocity = _moveDir * _moveSpeed;

			if (_moveDir != Vector2.zero)
			{
				AdjustRotation();
			}
		}

		private void AdjustRotation()
		{
			Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _moveDir);
			Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);

			_rb.SetRotation(rotation);
		}

		private void OnDied() => Debug.Log("im dead (Player.cs)");

		// INPUT
		private void OnMove(InputValue inputVal) => _moveDir = inputVal.Get<Vector2>();

		private void OnDash(InputValue _)
		{
			if (entity.dead || entity.invincible || Time.time - LastDashTime < DashCooldown) return;
			LastDashTime = Time.time;

			// get dash end position, blocked by walls
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position,
				transform.TransformDirection(Vector2.up),
				_dashDistance,
				Utils.wallLayerMask
			);
			
			Vector2 oldPos = transform.position;
			Vector2 endPos = hit.collider ? hit.point : (transform.position + transform.up * _dashDistance);
			transform.position = endPos;

			// afterimage by cloning the player sprite
			float magnitude = (oldPos - endPos).magnitude;
			for (float dist = 0; dist <= magnitude; dist += 0.4f)
			{
				Transform afterimage = Instantiate(
					_renderer,
					(Vector3)oldPos + transform.up * dist,
					transform.rotation,
					_persistent
				);
				SpriteRenderer afterRd = afterimage.GetComponent<SpriteRenderer>();

				afterimage.name = "afterimage";
				afterimage.localScale = _renderer.lossyScale * 0.9f;
				afterRd.color = new Color(1, 1, 1, 0.1f);
				Destroy(afterimage.gameObject, 2.0f);
			}
		}
	}
}