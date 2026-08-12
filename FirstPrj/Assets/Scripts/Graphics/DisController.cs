using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DisController : MonoBehaviour
{

    [SerializeField] Renderer targetRenderer;
    [SerializeField] string propertyName = "DissolveAmount";
    [SerializeField] float duration;

    float timer;

    void Start()
    {
        
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        targetRenderer.material.SetFloat(propertyName, t);
        //targetRenderer.sharedMaterial.SetFloat(propertyName, t);
    }
}
