using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BulletBrigade
{
	public class Tip : MonoBehaviour
	{
		public TMP_FontAsset fontAsset;
		public Color visColor = new Color(1, 1, 1, 1);
		public Color invisColor = new Color(1, 1, 1, 0);
		public string text = "[TEXT MISSING]";
		public float triggerDistance = 5f;

		private TMP_Text _spawnedTip;
		private Transform _plrTransform;
		private bool _triggered = false;

		private void Awake()
		{
			_plrTransform = GameObject.FindWithTag("Player").transform;
			var tempSpawnedTip = GameObject.Find("/UI/TipsFrame/TempSpawnedTip");

			_spawnedTip = Instantiate(
				tempSpawnedTip, tempSpawnedTip.transform.parent
				).GetComponent<TMP_Text>();

			_spawnedTip.text = text;
			_spawnedTip.font = fontAsset;
			tempSpawnedTip.GetComponent<TMP_Text>().color = _spawnedTip.color = invisColor;
		}

		private void Update()
		{
			_spawnedTip.transform.position = Camera.main.WorldToScreenPoint(transform.position);
			
			float mag = (_plrTransform.position - transform.position).magnitude;
			_spawnedTip.color = Color.Lerp(visColor, invisColor,
				Math.Min(1, Mathf.Clamp(mag / triggerDistance, 0, 1) * 2));

			// TODO: effect whilst triggered
			if (!_triggered && mag < triggerDistance) _triggered = true;
			else if (_triggered && mag > triggerDistance) _triggered = false;
		}

		private void OnDestroy()
		{
			Destroy(_spawnedTip);
		}
	}
}
