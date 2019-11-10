using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ErrorType
{
    CommandNotFound,
    SimpleCommandHasNestedCommand,
}

public struct CommandErrorStruct
{
    public ErrorType Type;
    public int LineNumber;
    public string ErrorMessage;
}
