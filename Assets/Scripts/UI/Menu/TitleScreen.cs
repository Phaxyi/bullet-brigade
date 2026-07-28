using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BulletBrigade
{
	/// <summary>
	/// Handles title screen and all functions stemming from it.
	/// </summary>
	public class TitleScreen : MonoBehaviour
	{
		private Game _levelInstance;
		private Transform _selectMenu;
		private Transform _selectButton;
		private bool _SMActive = false;
		private float _clickDebounce;
		
		private void Start()
		{
			_levelInstance = GetComponent<Game>();

			Transform _titleUI = GameObject.Find("/TitleUI").transform;
			_selectButton = _titleUI.Find("LevelSelectButton");
			_selectMenu = _titleUI.Find("LevelSelectMenu");
			_selectMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -700);

			Button PlayButton = _titleUI.Find("PlayButton").GetComponent<Button>();
			PlayButton.onClick.AddListener(() => FlashButton(PlayButton.transform));
			PlayButton.onClick.AddListener(StartNewGame);

			Button SMButton = _selectButton.GetComponent<Button>();
			SMButton.onClick.AddListener(() => FlashButton(SMButton.transform));
			SMButton.onClick.AddListener(SelectLevel);
		}

		public IEnumerator FlashButton(Transform buttonTrans)
		{
			// TODO: ref little busters UI and also play the same sound
			// TODO: make statnewgame itself wait
			TMP_Text text = buttonTrans.Find("Text").GetComponent<TMP_Text>();
			text.color = Color.white;
			text.DOColor(Color.black, 0.5f);

			yield return new WaitForSeconds(0.5f);
		}

		public void StartNewGame()
		{
			if (Time.time - _clickDebounce < 1f) return;
			_clickDebounce = Time.time;

			_levelInstance.StartNewGame();
		}

		public void SelectLevel()
		{
			if (Time.time - _clickDebounce < 1f) return;
			_clickDebounce = Time.time;

			_SMActive = !_SMActive;
			_selectMenu.GetComponent<RectTransform>().DOAnchorPosY(_SMActive ? 20 : -700, 0.75f);
			_selectButton.GetComponent<RectTransform>().DOAnchorPos(
				_SMActive ? new Vector3(-690, 30) : new Vector3(0, -156), 0.75f
			);
		}
	}
}
