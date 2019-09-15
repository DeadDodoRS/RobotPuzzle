using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandsMethods
{
    Forward,
    Backward,
    Turn,
    Do,
    While
}

public class CharacterCommand
{
    protected Character Character { get; private set; }
    public bool isRunning { get; set; } = false;
    public Action OnCommandEnd;

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
        OnCommandEnd?.Invoke();
    }
}
