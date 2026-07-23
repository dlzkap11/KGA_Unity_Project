using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private GameObject cameraRoot;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float topClamp;
    [SerializeField] private float bottomClamp;
    [SerializeField] private bool invertXAxis;

    private Vector2 lookInput;
    private Vector2 mouseDelta;
    private float targetX;
    private float targetY;

    private void Update()
    {
        CameraRotate();
    }

    public void OnLook(InputValue value)
    {
        mouseDelta = value.Get<Vector2>();
        lookInput = new Vector2(mouseDelta.x, mouseDelta.y);
    }

    private void CameraRotate()
    {
        // 좌우회전
        targetY += lookInput.x * rotateSpeed;

        // 상하회전
        if (invertXAxis)
            targetX += lookInput.y * rotateSpeed;
        else
            targetX -= lookInput.y * rotateSpeed;

        // 상하회전을 제한
        targetX = Mathf.Clamp(targetX, bottomClamp, topClamp);
        targetY = Mathf.Clamp(targetY, float.MinValue, float.MaxValue);

        // 좌우회전 값을 플레이어에게 적용
        transform.rotation = Quaternion.Euler(0f, targetY, 0f);

        // 상하회전 값을 카메라 루트에 적용
        cameraRoot.transform.localRotation = Quaternion.Euler(targetX, 0f, 0f);

    }
}