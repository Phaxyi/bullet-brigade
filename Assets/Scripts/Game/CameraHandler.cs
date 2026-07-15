using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletBrigade
{
	/// <summary>
	/// Handles custom camera movement following the player.
	/// </summary>
	public class CameraHandler : MonoBehaviour
	{
		// TODO: ideally not fully follow the player, do take into account level size
		// & allow player to be offset from centre of screen
		private Player _plr;

		private void Awake()
		{
			_plr = FindAnyObjectByType<Player>();
		}

		private void Update()
		{
			Transform camTrans = gameObject.transform;
			Vector2 newPos = Vector2.Lerp(	camTrans.position, _plr.transform.position, 0.025f	);
			
			// -10z to prevent weird artifacts
			camTrans.position = new Vector3(newPos.x, newPos.y, -10);
		}
	}
}
