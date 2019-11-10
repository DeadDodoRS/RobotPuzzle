using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class CommandControllerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _commandsContainer;

        public List<CommandUI> _firstLevelCommands = new List<CommandUI>();
        public CommandUI _defaultCommand;

        private void Awake()
        {
            InitializeDefaultCommand();
        }

        private void InitializeDefaultCommand()
        {
            //_defaultCommand = _commandsContainer.GetChild(0).GetComponent<CommandUI>();
            //_defaultCommand.gameObject.SetActive(false);

            //_firstLevelCommands.Add(_defaultCommand);
        }

        public CommandUI AddCommand(Transform parentTransform = null, CommandUI parentCommand = null)
        {
            AudioManager.Instance().Play(AudioClips.Click);

            CommandUI newCommand = FindDisableCommand(_firstLevelCommands);

            if (newCommand != null)
            {
                //If get disable command need remove it from parent command
                if (newCommand.ParentCommand == null)
                    _firstLevelCommands.Remove(newCommand);
                else
                    newCommand.ParentCommand.NestedCommands.Remove(newCommand);
                
                newCommand.gameObject.SetActive(true);
            }
            else
            {
                newCommand = SpawnNewCommand();
            }

            if (parentTransform == null)
            {
                newCommand.transform.parent = transform;

                newCommand.Initialize(this, null);
                _firstLevelCommands.Add(newCommand);
            }
            else
            {
                newCommand.transform.parent = parentTransform;

                newCommand.Initialize(this, parentCommand);
                parentCommand.NestedCommands.Add(newCommand);
            }

            return newCommand;
        }

        private CommandUI FindDisableCommand(List<CommandUI> commandLevel)
        {
            foreach (var command in commandLevel)
            {
                if (!command.gameObject.activeSelf)
                    return command;

                var anw = FindDisableCommand(command.NestedCommands);

                if (anw != null)
                    return anw;
            }

            return null;
        }

        private CommandUI SpawnNewCommand()
        {
            CommandUI newInput = Instantiate(_defaultCommand.gameObject).GetComponent<CommandUI>();
            return newInput;
        }

        public void EditCommand(CommandUI commandUI, string newTextValue)
        {
            BaseCommand command;
            if (GameController.Instance().CommandsController.TryGetSimpleCommand(newTextValue, out command))
            {
                commandUI.ErrorWave.gameObject.SetActive(false);

                if (commandUI.HasNestedCommands && (command is DoCommand || command is WhileCommand))
                {
                    commandUI.MakeParent();
                }

            }
            else
            {
                commandUI.ErrorWave.gameObject.SetActive(true);
            }
        }

        public void DeleteCommand(CommandUI command)
        {
            if (command.ParentCommand != null)
                command.ParentCommand.NestedCommands.Remove(command);
            else
                _firstLevelCommands.Remove(command);

            command.gameObject.SetActive(false);
        }

        public void ClearInput()
        {
            foreach (CommandUI input in _firstLevelCommands)
            {
                input.Clear();
                input.gameObject.SetActive(false);
            }
        }
    }
}
