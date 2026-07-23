using UnityEngine;

public class PlayerFloat : MonoBehaviour
{
    [SerializeField] Player player;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            player.isJumped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Float"))
            player.isJumped = false;

        Debug.Log(other.name);
    }
}
