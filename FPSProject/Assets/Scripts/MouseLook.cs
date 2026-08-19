using UnityEngine;
public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float angle;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private bool isInversed; //상하반전
    private float mouseX;
    private float mouseY;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

        playerBody.Rotate(Vector3.up * mouseX);

        if (!isInversed)
            pitch -= mouseY;
        else
            pitch += mouseY;

        pitch = Mathf.Clamp(pitch, -angle, angle);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
