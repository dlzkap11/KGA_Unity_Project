using UnityEngine;

public class MoveCommand : ICommand
{
    private GameObject target;
    private Vector3 delta;

    public MoveCommand(GameObject target, Vector3 delta)
    {
        this.target = target;
        this.delta = delta;
    }

    public void Execute()
    {
        target.transform.position += delta;
    }

    public void Undo()
    {
        target.transform.position -= delta;
    }
}
