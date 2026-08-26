using Unity.VisualScripting;
using UnityEngine;

public class MoveCommand : ICommand
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 delta;


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
