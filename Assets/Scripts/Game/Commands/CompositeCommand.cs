using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompositeCommand : BaseCommand
{
    protected List<BaseCommand> _commandsList;

    public CompositeCommand(Character implementator, List<BaseCommand> commands) : base(implementator)
    {
        _commandsList = commands?.ToList();
    }

    public void SetSubCommand(List<BaseCommand> commandsList)
    {
        _commandsList = commandsList;
    }

    public override void Execute()
    {

    }
    public override void Undo()
    {

    }
}
