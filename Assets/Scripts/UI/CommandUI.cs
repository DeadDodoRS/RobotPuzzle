using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CommandUI : MonoBehaviour
    {
        [Header("Base command")]
        [SerializeField] private TMP_InputField _commandField;
        [SerializeField] private Button _commandDelete;
        [SerializeField] private Image _errorWave;

        [Header("Nested commands")]
        [SerializeField] private RectTransform _containerNestedMenu;
        [SerializeField] private Button _addNestedCommand;
        [SerializeField] private RectTransform _containerNestedCommands;

        public List<CommandUI> NestedCommands = new List<CommandUI>();
        public bool HasNestedCommands;

        public string CommandText => _commandField.textComponent.text;
        public Image ErrorWave => _errorWave;
        public CommandUI ParentCommand { get; private set; }

        private CommandControllerUI _controller;

        public void Initialize(CommandControllerUI controller, CommandUI parentCommand)
        {
            _controller = controller;
            ParentCommand = parentCommand;

            _containerNestedMenu.gameObject.SetActive(false);
            _commandField.text = string.Empty;
        }

        public void MakeParent()
        {
            _containerNestedMenu.gameObject.SetActive(true);

            _addNestedCommand.onClick.AddListener(() => 
            {
                _controller.AddCommand(_containerNestedCommands, this);
            });
        }

        private void OnEnable()
        {
            _commandDelete.onClick.AddListener(() => _controller.DeleteCommand(this));
            _commandField.onValueChanged.AddListener(value =>
            {
                _controller.EditCommand(this, value);
            });
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
