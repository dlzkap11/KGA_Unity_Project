using UnityEngine;

public class Particle : MonoBehaviour
{
    ParticleSystem particle;
    GameObject particlePrefab;
    void Start()
    {
        if(particle == null)
        {
            particle = Instantiate(particlePrefab, particlePrefab.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            particle.Play();
        }
    }
}
