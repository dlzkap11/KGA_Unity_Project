using UnityEngine;

public class KeyboardCommander : MonoBehaviour
{
    [SerializeField] CommandInvoker commandInvoker;

    [SerializeField] GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ICommand command = new MoveCommand(player, Vector3.forward);
            commandInvoker.Execute(command);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ICommand command = new MoveCommand(player, Vector3.left);
            commandInvoker.Execute(command);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ICommand command = new MoveCommand(player, Vector3.back);
            commandInvoker.Execute(command);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ICommand command = new MoveCommand(player, Vector3.right);
            commandInvoker.Execute(command);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            commandInvoker.Undo();
        }
    }
}
