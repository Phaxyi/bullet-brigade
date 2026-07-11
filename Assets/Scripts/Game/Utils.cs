using UnityEngine;
using System;

namespace BulletBrigade {
	/// <summary>
	/// Various utility functions used in scripts.
	/// </summary>
	public class Utils : MonoBehaviour
	{
		public static LayerMask wallLayerMask;

		private static readonly float TAU = (float)Math.PI * 2;
		private static float _perlinStart;

		private void Awake()
		{
			wallLayerMask = LayerMask.GetMask("Wall");
			_perlinStart = Time.time;
		}

		public static float GetPerlinInterval(float seed, float speedMult)
		{
			float step = (Time.time - _perlinStart) * speedMult;
			float noise = Mathf.PerlinNoise(seed + step, seed - step);

			return Mathf.Clamp(noise, 0, 1) * TAU;
		}
	}
}