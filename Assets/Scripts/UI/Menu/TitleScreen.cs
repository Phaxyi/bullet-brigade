using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletBrigade
{
	/// <summary>
	/// Handles title screen and all functions stemming from it.
	/// </summary>
	public class TitleScreen : MonoBehaviour
	{
		private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);

		private Game _gameInstance;
		private Transform _selectButton;
		private Transform _selectMenu;
		private CanvasGroup _titleCanvasGroup;
		private Transform _tempLevelFrame;
		private bool LSActive = false;
		private float _clickDebounce = float.NegativeInfinity;
		
		private void Start()
		{
			_gameInstance = GetComponent<Game>();

			Transform titleUI = GameObject.Find("/TitleUI").transform;
			_selectButton = titleUI.Find("LevelSelectButton");
			_selectMenu = titleUI.Find("LevelSelectMenu");
			_titleCanvasGroup = titleUI.GetComponent<CanvasGroup>();

			Transform selectMenuContent = _selectMenu.Find("Viewport/Content");
			_tempLevelFrame = selectMenuContent.Find("TempLevelFrame");

			// init positions
			_selectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -156);
			_selectMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -720);

			// create level frames for Level Select
			int i = 0;
			foreach (LevelData level in Database.LevelData)
			{
				int levelID = i; // save i for AddListener
				Transform frame = Instantiate(_tempLevelFrame, selectMenuContent);
				frame.name = level.Name;

				frame.Find("LevelName").GetComponent<TMP_Text>().text = level.Name;
				frame.Find("LevelDesc").GetComponent<TMP_Text>().text = level.Desc;
				frame.GetComponent<Button>().onClick.AddListener(() => StartNewGame(levelID));
				i++;
			}

			// button functionality
			Button PlayButton = titleUI.Find("PlayButton").GetComponent<Button>();
			PlayButton.onClick.AddListener(() => FlashButton(PlayButton.transform));
			PlayButton.onClick.AddListener(() => StartNewGame());

			Button LSButton = _selectButton.GetComponent<Button>();
			LSButton.onClick.AddListener(() => FlashButton(LSButton.transform));
			LSButton.onClick.AddListener(SelectLevel);

			_tempLevelFrame.gameObject.SetActive(false);
		}

		public IEnumerator FlashButton(Transform buttonTrans)
		{
			// TODO: ref little busters UI and also play the same sound
			TMP_Text text = buttonTrans.Find("Text").GetComponent<TMP_Text>();
			text.color = Color.white;
			text.DOColor(Color.black, 0.5f);

			yield return _waitForSeconds0_5;
		}

		public void StartNewGame(int selectedLevel = -1)
		{
			if (Time.time - _clickDebounce < 0.75f) return;
			_clickDebounce = Time.time;

			_titleCanvasGroup.alpha = 0;
			_gameInstance.StartNewGame(selectedLevel);
		}

		public void SelectLevel()
		{
			if (Time.time - _clickDebounce < 0.75f) return;
			_clickDebounce = Time.time;
			LSActive = !LSActive;

			_selectMenu.GetComponent<RectTransform>().DOAnchorPosY(LSActive ? 20 : -720, 0.5f);
			_selectButton.GetComponent<RectTransform>().DOAnchorPos(
				LSActive ? new Vector3(-690, 130) : new Vector3(0, -156), 0.4f
			);
		}
	}
}
