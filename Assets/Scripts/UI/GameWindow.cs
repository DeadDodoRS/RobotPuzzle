using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameWindow : BaseWindow
    {
        [SerializeField] private RectTransform _commandsContainer;

        [SerializeField] private Button _addCommand;
        [SerializeField] private Button _runCommands;
        [SerializeField] private Button _clearAllCode;
        [SerializeField] private Button _toMainMenu;

        private List<TMP_InputField> _inputCommands = new List<TMP_InputField>();

        private void Awake()
        {
            _inputCommands.Add(_commandsContainer.GetChild(0).GetComponent<TMP_InputField>());
            _inputCommands[0].gameObject.SetActive(false);
        }

        public override void Open()
        {
            base.Open();

            GameController.Instance().GameOverAction += GameOverAction;

            _addCommand.onClick.AddListener(AddCommand);
            _runCommands.onClick.AddListener(RunCommands);
            _clearAllCode.onClick.AddListener(ClearInput);
            _toMainMenu.onClick.AddListener(ReturnToMainMenu);
        }

        public override void Close()
        {
            base.Close();

            GameController.Instance().GameOverAction -= GameOverAction;

            _addCommand.onClick.RemoveListener(AddCommand);
            _runCommands.onClick.RemoveListener(RunCommands);
            _clearAllCode.onClick.RemoveListener(ClearInput);
            _toMainMenu.onClick.RemoveListener(ReturnToMainMenu);
        }

        public void AddCommand()
        {
            AudioManager.Instance().Play(AudioClips.Click);

            foreach (var inputField in _inputCommands)
                if (!inputField.gameObject.activeSelf)
                {
                    inputField.gameObject.SetActive(true);
                    return;
                }

            var defaultInput = _commandsContainer.transform.GetChild(0);
            TMP_InputField newInput = Instantiate(defaultInput).GetComponent<TMP_InputField>();

            newInput.transform.parent = _commandsContainer.transform;
            _inputCommands.Add(newInput);
        }

        public void GameOverAction()
        {
            ClearInput();
        }

        public void ClearInput()
        {
            foreach (TMP_InputField input in _inputCommands)
            {
                input.textComponent.SetText(string.Empty);
                input.gameObject.SetActive(false);
            }
        }

        public void RunCommands()
        {
            AudioManager.Instance().Play(AudioClips.Click);

            foreach (TMP_InputField field in _inputCommands)
                GameController.Instance().CommandsController.AddNewCommand(field.text);

            GameController.Instance().StartCommands();
        }

        public void ReturnToMainMenu()
        {
            UIController.Instance().SetWindow(WindowsEnum.MainMenu);
            CameraController.Instance().SetMenuMode();
        }
    }
}
