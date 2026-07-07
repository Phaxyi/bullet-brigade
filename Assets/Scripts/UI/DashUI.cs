using TMPro;
using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Visualises player dashes.
	/// </summary>
	public class DashUI : MonoBehaviour
	{
		private Player _plr;
		private TMP_Text _dashText;

		private void Awake()
		{
			_plr = GameObject.FindWithTag("Player").GetComponent<Player>();
			_dashText = transform.Find("Text").GetComponent<TMP_Text>();
		}

		private void Update()
		{
			float timeSinceDash = Time.time - _plr.LastDashTime;
			_dashText.text = timeSinceDash < _plr.DashCooldown ? @$"dash ({
				Mathf.Round((_plr.DashCooldown - timeSinceDash) * 10) / 10
			})" : "";
		}
	}
}