using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RotateCommand : CharacterCommand {

    private RotateSides rotateSide;

    public RotateCommand(RotateSides side) : base ()
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
        if (rotateSide == RotateSides.LEFT)
            Character.Rotate(RotateSides.RIGHT, this);
        else
            Character.Rotate(RotateSides.LEFT, this);
    }
}
