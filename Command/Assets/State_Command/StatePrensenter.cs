using TMPro;
using UnityEngine;

public class StatePrensenter : MonoBehaviour
{
    [SerializeField] MonsterController controller;

    [SerializeField] TMP_Text stateText;

    private void OnEnable()
    {
        controller.OnStateChanged += Controller_OnStateChanged;
    }

    private void OnDisable()
    {
        controller.OnStateChanged -= Controller_OnStateChanged;
    }

    private void Start()
    {
        Controller_OnStateChanged(controller.CurState);
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void Controller_OnStateChanged(MonsterController.State obj)
    {
        stateText.text = obj.ToString();
    }
}
