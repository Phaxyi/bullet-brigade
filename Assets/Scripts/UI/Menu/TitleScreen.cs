using System.Collections;
using TMPro;
using UnityEngine;

namespace BulletBrigade
{
	/// <summary>
	/// Handles title screen and all functions stemming from it.
	/// </summary>
	public class TitleScreen : MonoBehaviour
	{
		private Game _levelInstance;
		private float _clickDebounce;
		
		private void Start()
		{
			_levelInstance = GetComponent<Game>();
		}

		public IEnumerator FlashButton(TMP_Text button = null)
		{
			// TODO: ref little busters UI and also play the same sound
			yield return new WaitForSeconds(0.5f);
		}

		public void StartNewGame()
		{
			if (Time.time - _clickDebounce < 1f) return;
			_clickDebounce = Time.time;

			StartCoroutine(FlashButton());
			_levelInstance.StartNewGame();
		}

		public void SelectLevel()
		{
			if (Time.time - _clickDebounce < 1f) return;
			_clickDebounce = Time.time;

			StartCoroutine(FlashButton());
			// TODO:
		}
	}
}
