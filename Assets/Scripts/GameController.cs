using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] public Character Player;

    #region Singleton
    private static GameController gameController;

    public static GameController Instance()
    {
        if (gameController == null)
            gameController = FindObjectOfType<GameController>();

        return gameController;
    }
    #endregion

    public bool IsGameRunning = false;
    public bool IsCommandsListRunning = false;
    public int CurrentLevel { get; set; } = -1;

    public Action LevelCompliteAction;
    public Action GameOverAction;

    public Queue<CharacterCommand> CommandList { get; set; } = new Queue<CharacterCommand>();
    public GameLevel[] Levels;

    public string CurrentLevelName { get { return Levels[CurrentLevel].Name; } }

    private CharacterCommand runningCommand;
    private bool CommandsRunning;

    public void StartLevel(bool isNextLevel)
    {
        if (CurrentLevel > -1)
            Levels[CurrentLevel].gameObject.SetActive(false);

        if (isNextLevel)
        {
            if (CurrentLevel == Levels.Length - 1)
                CurrentLevel = 0;
            else
                CurrentLevel += 1;
        }
        else
        {
            if (CurrentLevel == 0)
                CurrentLevel = Levels.Length - 1;
            else
                CurrentLevel -= 1;
        }

        Levels[CurrentLevel].gameObject.SetActive(true);
        Player.transform.position = Levels[CurrentLevel].StartPoint.position;
        Player.transform.rotation = Levels[CurrentLevel].StartPoint.rotation;
    }

    public bool CurrentLevelIsUnlocked()
    {
        if (CurrentLevel == 0 || CurrentLevel == 3)
            return true;

        return false;
    }

    public void StartCommands()
    {
        CommandsRunning = true;
    }

    public void AddNewCommand(string command)
    {
        foreach (Commands com in Enum.GetValues(typeof(Commands)))
        {
            if (command == com.ToString())
            {
                switch (com)
                {
                    case (Commands.Forward): CommandList.Enqueue(new MoveCommand(MoveSides.FORWARD)); break;
                    case (Commands.Backward): CommandList.Enqueue(new MoveCommand(MoveSides.BACKWARD)); break;
                    case (Commands.Turn_left): CommandList.Enqueue(new RotateCommand(RotateSides.LEFT)); break;
                    case (Commands.Turn_right): CommandList.Enqueue(new RotateCommand(RotateSides.RIGHT)); break;
                }

                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (CommandsRunning)
            RunCommands();
    }

    private void RunCommands()
    {
        if (runningCommand != null && runningCommand.isRunning)
            return;

        if (CommandList.Count == 0)
        {
            CommandsRunning = false;
            return;
        }

        runningCommand = CommandList.Dequeue();
        runningCommand.Execute();
    }

    public void EndLevel(bool isWin = false)
    {
        IsGameRunning = false;

        if (isWin)
            LevelComplite();
        else
            GameOver();
    }

    private void LevelComplite()
    {
        LevelCompliteAction();
    }

    private void GameOver()
    {
        Vector3 restartPos = Levels[CurrentLevel].StartPoint.position;
        Player.EndCommand();

        Player.transform.rotation = Levels[CurrentLevel].StartPoint.rotation;
        Player.transform.position = new Vector3(restartPos.x, restartPos.y + 3, restartPos.z);
        CommandList = new Queue<CharacterCommand>();

        GameOverAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        EndLevel();
    }

}
