using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletBrigade {
	/// <summary>
	/// Visualises player health.
	/// </summary>
	public class HealthbarUI : MonoBehaviour
	{
		private Player _plr;
		private Entity _plrEntity;
		private Image _barImage;
		private Image[] _hearts;
		private TMP_Text _barText;

		private const float LERP_TIME = 0.6f;
		private float targetFill;
		private float lerpStart = Mathf.NegativeInfinity;

		private void Awake()
		{
			GameObject playerObj = GameObject.FindWithTag("Player");
			_plr = playerObj.GetComponent<Player>();
			_plrEntity = playerObj.GetComponent<Entity>();

			_barImage = transform.Find("Bar").GetComponent<Image>();
			_barText = transform.Find("Value").GetComponent<TMP_Text>();
			_hearts = transform.Find("HeartHolder").GetComponentsInChildren<Image>();
		}

		private void Update()
		{
			// healthbar
			float newTargetFill = _plrEntity.health / _plrEntity.maxHealth;
			if (targetFill != newTargetFill)
			{
				targetFill = Mathf.Clamp(newTargetFill, 0, 1);
				lerpStart = Time.time;

				_barText.text = _plrEntity.health.ToString();
			}
			
			float t = (Time.time - lerpStart) / LERP_TIME;
			_barImage.fillAmount = Mathf.SmoothStep(_barImage.fillAmount, targetFill, t);
			_barImage.color = _plr.usingSafeZone ? Color.softGreen : Color.red;
			
			// display hearts
			int i = 1;
			foreach (Image heart in _hearts)
			{
				heart.color = Level.Hearts >= i ? Color.red : Color.black;
				i++;
			}
		}
	}
}