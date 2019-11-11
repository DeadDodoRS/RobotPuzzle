using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoCommand : CompositeCommand
{
    private int _repeatCount;
    private int _currentRepeatCount;
    private int _currentCommandIndex;

    public DoCommand(Character implementator, int repeatCount) : base(implementator)
    {
        _repeatCount = repeatCount;
    }

    public override void Execute()
    {
        _currentCommandIndex = 0;
        _currentRepeatCount = _repeatCount;

        ExecuteNextCommand();
    }

    private void ExecuteNextCommand()
    {
        if (_currentCommandIndex != 0)
        {
            _commandsList[_currentCommandIndex - 1].OnCommandEnd -= ExecuteNextCommand;
        }

        if (_currentCommandIndex >= _commandsList.Count)
        {
            //Check end of cycle
            if (_currentRepeatCount == 0)
            {
                _currentCommandIndex = 0;
                CommandEnd();
                return;
            }
            else
            {
                _currentRepeatCount--;
                _currentCommandIndex = 0;
            }
        }

        _commandsList[_currentCommandIndex].Execute();
        _commandsList[_currentCommandIndex].OnCommandEnd += ExecuteNextCommand;
        _currentCommandIndex++;
    }

    public override void Undo()
    {

    }
}
