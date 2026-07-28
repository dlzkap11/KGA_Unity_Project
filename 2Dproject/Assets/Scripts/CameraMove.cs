using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 Pos;
    float zOffset = -10f;

    void LateUpdate()
    {

        Pos = new Vector3(player.position.x, player.position.y, zOffset);
        transform.position = Vector3.Lerp(transform.position, Pos, 0.1f);
        //transform.position = Pos;
    }
}
