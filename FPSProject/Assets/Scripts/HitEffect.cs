using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitEffectPrefab;

    public void TakeHit(Vector3 pos, Vector3 normal)
    {
        ParticleSystem hitEffect = Instantiate(hitEffectPrefab, pos, Quaternion.identity);
        hitEffect.transform.forward = normal;
        hitEffect.transform.parent = transform;
    }
}
