using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class CommandControllerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _commandsContainer;

        private List<CommandUI> _inputCommands = new List<CommandUI>();

        private void Awake()
        {
            _inputCommands.Add(_commandsContainer.GetChild(0).GetComponent<CommandUI>());
            _inputCommands[0].gameObject.SetActive(false);
            _inputCommands[0].Initialize(this);
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
            CommandUI newInput = Instantiate(defaultInput).GetComponent<CommandUI>();

            newInput.transform.parent = _commandsContainer.transform;
            newInput.Initialize(this);
            _inputCommands.Add(newInput);
        }

        public void EditCommand(CommandUI commandUI, string newTextValue)
        {
            BaseCommand command;
            if (GameController.Instance().CommandsController.TryGetCommand(newTextValue, out command))
            {
                commandUI.ErrorWave.gameObject.SetActive(false);
            } else
            {
                commandUI.ErrorWave.gameObject.SetActive(true);
            }
        }

        public void DeleteCommand(CommandUI command)
        {
            _inputCommands.Remove(command);
            _inputCommands.Add(command);

            command.gameObject.SetActive(false);
        }

        public void ClearInput()
        {
            foreach (CommandUI input in _inputCommands)
            {
                input.Clear();
                input.gameObject.SetActive(false);
            }
        }
    }
}
