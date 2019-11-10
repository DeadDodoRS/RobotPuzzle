using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MBSingleton<GameController>
{
    [SerializeField] public Character Player;
    [SerializeField] private GameLevel[] Levels;
    public CommandsController CommandsController;

    public Action<GameLevel> LevelChange;
    public Action LevelCompliteAction;
    public Action GameOverAction;

    public int CurrentLevel { get; private set; } = -1;
    public string CurrentLevelName => Levels[CurrentLevel].Name;

    private void Awake()
    {
        SetLevel(0);
        CommandsController = new CommandsController(Player);
    }

    public int GetNearestLevel(bool isNext)
    {
        return isNext ? (CurrentLevel == Levels.Length - 1 ? 0 : CurrentLevel + 1)
                      : (CurrentLevel == 0 ? Levels.Length - 1 : CurrentLevel - 1);
    }

    public void StartCommands()
    {
        CommandsController.StartCommands();
    }

    private void Update()
    {
        if (CommandsController.IsCommandsRunning)
            CommandsController.RunCommands();
    }

    public void FinishLevel(bool isSuccess = false)
    {
        if (isSuccess)
            LevelComplite();
        else
            LevelFail();
    }

    private void LevelComplite()
    {
        StartCoroutine(ChangeLevelDelay());
    }

    private IEnumerator ChangeLevelDelay()
    {
        yield return new WaitForSeconds(1f);
        SetLevel(GetNearestLevel(true));
        AudioManager.Instance().Play(AudioClips.Click);
    }

    private void LevelFail()
    {
        Player.StopRunning();

        Vector3 spawnPosition = Levels[CurrentLevel].StartPoint.position;
        Player.transform.rotation = Levels[CurrentLevel].StartPoint.rotation;

        Player.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + 3, spawnPosition.z);
        CommandsController.Clear();

        GameOverAction();
    }

    public void SetLevel(int levelIndex)
    {
        if (CurrentLevel >= 0)
            Levels[CurrentLevel].gameObject.SetActive(false);

        CurrentLevel = levelIndex;
        Levels[CurrentLevel].gameObject.SetActive(true);
        Player.transform.position = Levels[CurrentLevel].StartPoint.position;
        Player.transform.rotation = Levels[CurrentLevel].StartPoint.rotation;

        LevelChange?.Invoke(Levels[CurrentLevel]);
    }
}
