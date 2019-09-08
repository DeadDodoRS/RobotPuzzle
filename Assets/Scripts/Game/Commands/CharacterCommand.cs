using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandsMethods
{
    Forward,
    Backward,
    Turn,
}

public class CharacterCommand
{
    protected Character Character { get; private set; }
    public bool isRunning { get; set; } = false;

    protected CharacterCommand(Character implementator)
    {
        Character = implementator;
        isRunning = true;
    }

    public virtual void Execute() { }
    public virtual void Undo() { }

    public virtual void CommandEnd()
    {
        isRunning = false;
    }
}
