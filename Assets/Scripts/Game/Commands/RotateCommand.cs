using System;

public enum TurnArguments
{
    Left,
    Right,
}

public class RotateCommand : CharacterCommand {

    private TurnArguments rotateSide;

    public RotateCommand(Character implementator, TurnArguments side) : base (implementator)
    {
        rotateSide = side;
    }

    public override void Execute()
    {
        Character.Rotate(rotateSide, this);
    }

    public override void Undo()
    {
        //TO-DO another moveside
        if (rotateSide == TurnArguments.Left)
            Character.Rotate(TurnArguments.Right, this);
        else
            Character.Rotate(TurnArguments.Left, this);
    }
}
