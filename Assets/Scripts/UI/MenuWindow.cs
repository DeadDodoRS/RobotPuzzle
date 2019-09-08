using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _levelName;

        [SerializeField] private Button _goToGame;
        [SerializeField] private Button _prevLevel;
        [SerializeField] private Button _nextLevel;

        [SerializeField] private Image _lockedScreen;

        public override void Open()
        {
            base.Open();
            _goToGame.onClick.AddListener(StartGame);

            _prevLevel.onClick.AddListener(() => GameController.Instance().SetLevel(GameController.Instance().GetNearestLevel(false)));
            _nextLevel.onClick.AddListener(() => GameController.Instance().SetLevel(GameController.Instance().GetNearestLevel(true)));
            GameController.Instance().LevelChange += OnChangeLevel;
        }

        public override void Close()
        {
            base.Close();
            _goToGame.onClick.RemoveListener(StartGame);

            _prevLevel.onClick.RemoveAllListeners();
            _nextLevel.onClick.RemoveAllListeners();

            GameController.Instance().LevelChange -= OnChangeLevel;
        }

        private void StartGame()
        {
            UIController.Instance().SetWindow(WindowsEnum.InGame);
            CameraController.Instance().SetGameMode();
        }

        private void OnChangeLevel(GameLevel level)
        {
            //Check needed lockscreen
            if (!CurrentLevelIsUnlocked() && _lockedScreen.gameObject.activeSelf == false)
                _lockedScreen.gameObject.SetActive(true);
            else if (_lockedScreen.gameObject.activeSelf == true)
                _lockedScreen.gameObject.SetActive(false);

            _levelName.SetText(level.Name);
        }

        private bool CurrentLevelIsUnlocked()
        {
            //if (CurrentLevel == 0 || CurrentLevel == 3)
            //    return true;

            return true;
        }
    }
}
