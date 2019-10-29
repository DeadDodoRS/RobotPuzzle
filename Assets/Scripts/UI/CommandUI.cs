using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CommandUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _commandField;
        [SerializeField] private Button _commandDelete;
        [SerializeField] private Image _errorWave;
        public string CommandText => _commandField.textComponent.text;
        public Image ErrorWave => _errorWave;

        private CommandControllerUI _controller;
        public void Initialize(CommandControllerUI controller)
        {
            _controller = controller;
        }

        private void OnEnable()
        {
            _commandDelete.onClick.AddListener(() => _controller.DeleteCommand(this));
            _commandField.onValueChanged.AddListener(value => _controller.EditCommand(this, value));
        }

        public void Clear()
        {
            _commandField.textComponent.SetText(string.Empty);
        }

        private void OnDisable()
        {
            _commandDelete.onClick.RemoveAllListeners();
            _commandField.onValueChanged.RemoveAllListeners();
        }
    }
}
