using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Spins the gameObject it's linked to.
	/// </summary>
	public class Spinner : MonoBehaviour
	{
		// TODO: like a Lot of configs
		[SerializeField] private float _fullSpinDuration = 1;

		private void FixedUpdate()
		{
			// TODO: wrong duration
			float degrees = Time.fixedDeltaTime * 360 / _fullSpinDuration;
			transform.Rotate(Vector3.forward * degrees);
		}
	}
}
