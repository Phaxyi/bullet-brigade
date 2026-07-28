using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
		private Transform _selectButton;
		private Transform _selectMenu;
		private Transform _tempLevelFrame;
		private bool LSActive = false;
		private float _clickDebounce;
		
		private void Start()
		{
			_levelInstance = GetComponent<Game>();

			Transform titleUI = GameObject.Find("/TitleUI").transform;
			_selectButton = titleUI.Find("LevelSelectButton");
			_selectMenu = titleUI.Find("LevelSelectMenu");

			Transform selectMenuContent = _selectMenu.Find("Viewport/Content");
			_tempLevelFrame = selectMenuContent.Find("TempLevelFrame");

			// init positions
			_selectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -156);
			_selectMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -720);

			// create level frames for Level Select
			foreach (LevelData level in Database.LevelData)
			{
				Transform frame = Instantiate(_tempLevelFrame, selectMenuContent);
				frame.name = level.Name;

				frame.Find("LevelName").GetComponent<TMP_Text>().text = level.Name;
				frame.Find("LevelDesc").GetComponent<TMP_Text>().text = level.Desc;
			}

			// button functionality
			Button PlayButton = titleUI.Find("PlayButton").GetComponent<Button>();
			PlayButton.onClick.AddListener(() => FlashButton(PlayButton.transform));
			PlayButton.onClick.AddListener(StartNewGame);

			Button LSButton = _selectButton.GetComponent<Button>();
			LSButton.onClick.AddListener(() => FlashButton(LSButton.transform));
			LSButton.onClick.AddListener(SelectLevel);

			_tempLevelFrame.gameObject.SetActive(false);
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
			LSActive = !LSActive;

			_selectMenu.GetComponent<RectTransform>().DOAnchorPosY(LSActive ? 20 : -720, 0.5f);
			_selectButton.GetComponent<RectTransform>().DOAnchorPos(
				LSActive ? new Vector3(-690, 130) : new Vector3(0, -156), 0.4f
			);
		}
	}
}
