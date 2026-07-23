using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private bool isAct;
    Player player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            player = other.GetComponent<Player>();
            player.StartPos = transform.position + new Vector3(0, 1f, 0f);

            isAct = true;
            GetComponent<Renderer>().material.color = Color.blue;
        }
            
    }
}
