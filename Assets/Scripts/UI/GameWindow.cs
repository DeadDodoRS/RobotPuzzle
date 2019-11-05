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
                new[] { WhileConditions.NotCheckpoint.ToString()});
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
            AudioManager.Instance().Play(AudioClips.Click);

            GameController.Instance().CommandsController.SetCommandsList(_commandsController._firstLevelCommands);
        }

        public void ReturnToMainMenu()
        {
            UIController.Instance().SetWindow(WindowsEnum.MainMenu);
            CameraController.Instance().SetMenuMode();
        }
    }
}
