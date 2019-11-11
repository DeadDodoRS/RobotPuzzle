using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameWindow : BaseWindow
    {
        [Header("Buttons")]
        [SerializeField] private Button _addCommand;
        [SerializeField] private Button _runCommands;
        [SerializeField] private Button _clearAllCode;
        [SerializeField] private Button _toMainMenu;
        [SerializeField] private Button _openHelper;

        [Header("Containers")]
        [SerializeField] private CommandControllerUI _commandsController;
        [SerializeField] private RectTransform _containerHelper;
        [SerializeField] private RectTransform _containerCommandExamples;

        private List<CommandErrorStruct> errorList;
        public virtual void Awake()
        {
            _containerCommandExamples.transform.GetChild(0).GetComponent<CommandExampleUI>()
                .Initialize(CommandsMethods.Forward.ToString());

            CreateNewCommandExample().Initialize(CommandsMethods.Backward.ToString());

            CreateNewCommandExample().Initialize(CommandsMethods.Turn.ToString(),
                new[] { TurnArguments.Left.ToString(), TurnArguments.Right.ToString() });

            CreateNewCommandExample().Initialize(CommandsMethods.Do.ToString(),
                new[] {"(int)count"});

            CreateNewCommandExample().Initialize(CommandsMethods.While.ToString(), 
                new[] { WhileConditions.NotPlate.ToString()});
        }

        private CommandExampleUI CreateNewCommandExample()
        {
            var defaultExample = _containerCommandExamples.transform.GetChild(0);
            CommandExampleUI newExample = Instantiate(defaultExample).GetComponent<CommandExampleUI>();
            newExample.transform.parent = _containerCommandExamples.transform;
            return newExample;
        }

        public override void Open()
        {
            base.Open();

            GameController.Instance().GameOverAction += _commandsController.ClearInput;

            _addCommand.onClick.AddListener(() => _commandsController.AddCommand());
            _runCommands.onClick.AddListener(RunCommands);
            _clearAllCode.onClick.AddListener(_commandsController.ClearInput);
            _toMainMenu.onClick.AddListener(ReturnToMainMenu);
            _openHelper.onClick.AddListener(() => _containerHelper.gameObject.SetActive(!_containerHelper.gameObject.activeSelf));
        }

        public override void Close()
        {
            base.Close();

            GameController.Instance().GameOverAction -= _commandsController.ClearInput;

            _addCommand.onClick.RemoveAllListeners();
            _runCommands.onClick.RemoveAllListeners();
            _clearAllCode.onClick.RemoveAllListeners();
            _toMainMenu.onClick.RemoveAllListeners();
            _openHelper.onClick.RemoveAllListeners();
        }

        public void RunCommands()
        {
            errorList = new List<CommandErrorStruct>();

            AudioManager.Instance().Play(AudioClips.Click);

            GameController.Instance().CommandsController.CommandStartError += WriteError;
            GameController.Instance().CommandsController.TryStartCommandList(_commandsController._firstLevelCommands);
            GameController.Instance().CommandsController.CommandStartError -= WriteError;

            foreach(var t in GameController.Instance().CommandsController.CommandList)
            {
                Debug.Log($"Command: {t.GetType()}");
            }

            if (errorList.Count > 0)
            {
                UIController.Instance().SetWindow(WindowsEnum.Error, config: new ErrorWindowConfig() { errors = errorList });
                return;
            }

            GameController.Instance().StartCommands();
        }

        private void WriteError(ErrorType type)
        {
            errorList.Add(new CommandErrorStruct() { 
                Type = type,
                LineNumber = GameController.Instance().CommandsController._currentCommandLine
            });
        }

        public void ReturnToMainMenu()
        {
            UIController.Instance().SetWindow(WindowsEnum.MainMenu);
            CameraController.Instance().SetMenuMode();
        }
    }
}
