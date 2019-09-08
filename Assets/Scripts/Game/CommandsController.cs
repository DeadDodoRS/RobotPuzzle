using System;
using System.Collections.Generic;
public class CommandsController
{
    public Queue<CharacterCommand> CommandList { get; set; } = new Queue<CharacterCommand>();
    public bool CommandsRunning { get; private set; }

    private CharacterCommand _runningCommand;
    private Character _implementator;

    public CommandsController(Character implementator)
    {
        _implementator = implementator;
    }

    public void AddNewCommand(string command)
    {
        string[] arguments = null;
        string methodName = GetMethodSingature(command, ref arguments);

        foreach (CommandsMethods com in Enum.GetValues(typeof(CommandsMethods)))
        {
            if (methodName == com.ToString())
            {
                switch (com)
                {
                    case (CommandsMethods.Forward): CommandList.Enqueue(new MoveCommand(_implementator, MoveSides.FORWARD)); return;
                    case (CommandsMethods.Backward): CommandList.Enqueue(new MoveCommand(_implementator, MoveSides.BACKWARD)); return;
                    case (CommandsMethods.Turn):
                        {
                            if (arguments.Length == 0)
                                return;

                            var turnSide = arguments[0].ToLower();

                            if (turnSide == TurnArguments.Right.ToString().ToLower())
                                CommandList.Enqueue(new RotateCommand(_implementator, TurnArguments.Right));
                            else if (turnSide == TurnArguments.Right.ToString().ToLower())
                                CommandList.Enqueue(new RotateCommand(_implementator, TurnArguments.Left));
                            return;
                        }
                }

                break;
            }
        }
    }

    private string GetMethodSingature(string command, ref string[] arguments)
    {
        int methodNameIndex = command.IndexOf('(');

        if (methodNameIndex == -1)
            return command;

        // +1 ; -2 For remove symbols '(' and ')'
        arguments = command.Substring(methodNameIndex + 1, command.Length - methodNameIndex - 2)
            .Replace(" ", string.Empty)
            .Split(',');
        return command.Substring(0, methodNameIndex);
    }

    public void RunCommands()
    {
        if (_runningCommand != null && _runningCommand.isRunning)
            return;

        if (CommandList.Count == 0)
        {
            CommandsRunning = false;
            return;
        }

        _runningCommand = CommandList.Dequeue();
        _runningCommand.Execute();
    }

    public void StartCommands()
    {
        CommandsRunning = true;
    }

    public void Clear()
    {
        CommandList = new Queue<CharacterCommand>();
    }
}
