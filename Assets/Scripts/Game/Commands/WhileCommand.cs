using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WhileConditions
{
    NotCheckpoint
}

public class WhileCommand : BaseCommand
{
    private List<BaseCommand> _commandsList;
    private int _currentCommandIndex;

    public WhileCommand(Character implementator, List<BaseCommand> commands, WhileConditions condition) : base(implementator)
    {
        _commandsList = commands.ToList();
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
