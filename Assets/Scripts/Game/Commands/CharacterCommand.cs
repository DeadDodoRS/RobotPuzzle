using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Commands
{
    Forward,
    Backward,
    Turn_left,
    Turn_right,
}

public class CharacterCommand
{
    protected Character Character { get; private set; }
    public bool isRunning { get; set; } = false;

    protected CharacterCommand()
    {
        Character = GameController.Instance().Player;
        isRunning = true;
    }

    public virtual void Execute() { }
    public virtual void Undo() { }

    public virtual void CommandEnd()
    {
        isRunning = false;
    }
}
