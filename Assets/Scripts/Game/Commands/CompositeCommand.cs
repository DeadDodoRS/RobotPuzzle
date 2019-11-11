using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompositeCommand : BaseCommand
{
    protected List<BaseCommand> _commandsList;

    public CompositeCommand(Character implementator) : base(implementator)
    {
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
