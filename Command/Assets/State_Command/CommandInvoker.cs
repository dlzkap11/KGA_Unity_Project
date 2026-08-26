using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    private Stack<ICommand> history = new Stack<ICommand>();

    public void Execute(ICommand command)
    {
        command.Execute();
        history.Push(command);
    }

    public void Undo()
    {
        if (history.Count == 0)
            return;

        ICommand command = history.Pop();
        command.Undo();
    }
}
