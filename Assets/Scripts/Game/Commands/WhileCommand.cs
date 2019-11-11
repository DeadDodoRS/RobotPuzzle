using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WhileConditions
{
    NotPlate = 1,
}

public class WhileCommand : CompositeCommand
{
    private int _currentCommandIndex;

    public WhileCommand(Character implementator, WhileConditions condition) : base(implementator)
    {
        _currentCommandIndex = -1;
    }

    public override void Execute()
    {
        ExecuteNextCommand();
    }

    private void ExecuteNextCommand()
    {
        _currentCommandIndex += 1;
        _commandsList[_currentCommandIndex].Execute();
        _commandsList[_currentCommandIndex].OnCommandEnd += ExecuteNextCommand;
    }

    public override void Undo()
    {

    }
}
