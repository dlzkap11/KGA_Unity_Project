using UnityEngine;

public class GraphicsProbe : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Light mainLight;
    [SerializeField] private Material material;
    [SerializeField] private float rotSpeed;


    void Start()
    {
        //meshRenderer.material = material;
        meshRenderer.sharedMaterial = material;
        meshRenderer.sharedMaterial.color = mainLight.color;
    }

    
    void Update()
    {
        mainLight.transform.Rotate(Vector3.right * rotSpeed * Time.deltaTime,Space.World);
    }
}
