using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    Stack<ICommand> history = new Stack<ICommand>();


    public void Execute(ICommand command)
    {
        command.Execute();
       history.Push(command);
    }

    public void UndoLast()
    {
        if (history.Count < 0)
            return;


    }

}
