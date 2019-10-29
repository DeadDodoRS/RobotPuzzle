using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandsMethods
{
    Forward = 1,
    Backward = 2,
    Turn = 3,
    Do = 4,
    While = 5,
}

public class BaseCommand
{
    protected Character Character { get; private set; }
    public bool isRunning { get; set; } = false;
    public Action OnCommandEnd;

    protected BaseCommand(Character implementator)
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
