using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoCommand : CompositeCommand
{
    private int _repeatCount;
    private int _currentCommandIndex;

    public DoCommand(Character implementator, List<BaseCommand> commands, int repeatCount) : base(implementator, commands)
    {
        _repeatCount = repeatCount;
    }

    public override void Execute()
    {
        _currentCommandIndex = 0;
        _repeatCount--;

        ExecuteNextCommand();
    }

    private void ExecuteNextCommand()
    {
        if(_currentCommandIndex != 0)
            _commandsList[_currentCommandIndex - 1].OnCommandEnd -= ExecuteNextCommand;

        if (_currentCommandIndex >= _commandsList.Count)
        {
            //Check end of cycle
            if (_repeatCount == 0)
            {
                CommandEnd();
                return;
            }
            else
            {
                _repeatCount--;
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
