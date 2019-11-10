using System;
using System.Collections.Generic;
using System.Linq;
using UI;

public class CommandsController
{
    public Queue<BaseCommand> CommandList { get; set; } = new Queue<BaseCommand>();
    public bool IsCommandsRunning { get; private set; }

    private List<CommandsMethods> _availableCommands;
    private BaseCommand _currentCommand;
    private Character _implementator;

    public int _currentCommandLine;

    public Action<ErrorType> CommandStartError;

    private bool _findError;

    public CommandsController(Character implementator)
    {
        _implementator = implementator;

        _availableCommands = Enum.GetValues(typeof(CommandsMethods))
            .Cast<CommandsMethods>()
            .ToList();
    }

    public void StartCommands()
    {
        IsCommandsRunning = true;
    }

    public void TryStartCommandList(List<CommandUI> commandList)
    {
        CommandList.Clear();

        _currentCommandLine = 0;
        _findError = false;

        CommandStartError += StopStartCommandList;

        foreach (var container in commandList)
        {
            if (_findError)
                break;

            BaseCommand newCommand;
            if(TryGetRealCommand(container, out newCommand))
                CommandList.Enqueue(newCommand);
        }

        CommandStartError -= StopStartCommandList;
    }

    private void StopStartCommandList(ErrorType error)
    {
        _findError = false;
        CommandStartError -= StopStartCommandList;
    }

    private bool TryGetRealCommand(CommandUI commandUI, out BaseCommand returnCommand)
    {
        returnCommand = null;

        _currentCommandLine++;

        if (!GameController.Instance().CommandsController.TryGetSimpleCommand(commandUI.CommandText, out returnCommand))
        {
            CommandStartError.Invoke(ErrorType.CommandNotFound);
            return false;
        }

        if (returnCommand is CompositeCommand)
        {
            List<BaseCommand> nestedCommands = new List<BaseCommand>();

            foreach (var com in commandUI.NestedCommands)
            {
                if (_findError)
                    return false;

                BaseCommand findedCommand;

                if (TryGetRealCommand(com, out findedCommand))
                    nestedCommands.Add(findedCommand);
            }

            (returnCommand as CompositeCommand).SetSubCommand(nestedCommands);
        }
        else
        {
            if (commandUI.NestedCommands != null && commandUI.NestedCommands.Count > 0) 
            { 
                CommandStartError.Invoke(ErrorType.SimpleCommandHasNestedCommand);
                return false;
            }
        }

        return true;
    }

    public bool TryGetSimpleCommand(string command, out BaseCommand returnCommand)
    {
        returnCommand = null;
        string[] arguments = null;
        string methodName = GetMethodSingature(command, ref arguments);

        CommandsMethods commandEnum = _availableCommands.FirstOrDefault(com => com.ToString() == methodName);

        if (commandEnum == 0)
            return false;

        switch (commandEnum)
        {
            case (CommandsMethods.Forward): 
                returnCommand = new MoveCommand(_implementator, MoveSides.FORWARD);
                return true;
            case (CommandsMethods.Backward): 
                returnCommand = new MoveCommand(_implementator, MoveSides.BACKWARD);
                return true;
            case (CommandsMethods.Turn):
                {
                    if (arguments == null || arguments.Length == 0)
                        return false;

                    var turnSide = arguments[0].ToLower();

                    if (turnSide == TurnArguments.Right.ToString().ToLower())
                        returnCommand = new RotateCommand(_implementator, TurnArguments.Right);
                    else if (turnSide == TurnArguments.Left.ToString().ToLower())
                        returnCommand = new RotateCommand(_implementator, TurnArguments.Left);
                    return false;
                }
            case (CommandsMethods.Do):
                {
                    int count = 0;
                    if (arguments == null || arguments.Length == 0 || !int.TryParse(arguments[0], out count))
                        return false;

                    returnCommand = new DoCommand(_implementator, null, count);
                    return true;
                }
            case (CommandsMethods.While):
                {
                    if (arguments == null || arguments.Length == 0)
                        return false;



                    break;
                }
        }

        return false;
    }

    private string GetMethodSingature(string command, ref string[] arguments)
    {
        int methodParametersStart = command.IndexOf('(');
        int methodParametersFinish = command.IndexOf(')');

        if (methodParametersStart == -1  || methodParametersStart >= methodParametersFinish)
            return command;

        // +1 ; -1 remove symbols '(' and ')'
        arguments = command.Substring(methodParametersStart + 1, methodParametersFinish - methodParametersStart - 1)
            .Replace(" ", string.Empty)
            .Split(',');
        return command.Substring(0, methodParametersStart);
    }

    public void Clear()
    {
        CommandList = new Queue<BaseCommand>();
    }

    public void RunCommands()
    {
        if (_currentCommand != null && _currentCommand.isRunning)
            return;

        if (CommandList.Count == 0)
        {
            IsCommandsRunning = false;
            return;
        }

        _currentCommand = CommandList.Dequeue();
        _currentCommand.Execute();
    }
}
