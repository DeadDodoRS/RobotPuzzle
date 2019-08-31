using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : CharacterCommand
{
    private MoveSides moveSide;

    public MoveCommand(MoveSides sides) : base ()
    {
        moveSide = sides;
    }

    public override void Execute()
    {
        Character.Move(moveSide, this);
    }

    public override void Undo()
    {
        //TO-DO another moveside
        if (moveSide == MoveSides.FORWARD)
            Character.Move(MoveSides.BACKWARD, this);
        else
            Character.Move(MoveSides.FORWARD, this);
    }
}

