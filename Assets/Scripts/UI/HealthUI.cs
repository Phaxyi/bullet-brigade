using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletBrigade {
	public class HealthbarUI : MonoBehaviour
	{
		private Entity _plrEntity;
		private Image _barImage;
		private TMP_Text _barText;

		private const float LERP_TIME = 0.6f;
		private float targetFill;
		private float lerpStart = Mathf.NegativeInfinity;

		private void Awake()
		{
			_plrEntity = GameObject.FindWithTag("Player").GetComponent<Entity>();

			_barImage = transform.Find("Bar").GetComponent<Image>();
			_barText = transform.Find("Value").GetComponent<TMP_Text>();
		}

		private void Update()
		{
			float newTargetFill = _plrEntity.health / _plrEntity.maxHealth;
			if (targetFill != newTargetFill)
			{
				targetFill = Mathf.Clamp(newTargetFill, 0, 1);
				lerpStart = Time.time;

				_barText.text = _plrEntity.health.ToString();
			}
			
			float t = (Time.time - lerpStart) / LERP_TIME;
			_barImage.fillAmount = Mathf.SmoothStep(_barImage.fillAmount, targetFill, t);
		}
	}
}