using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider), typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    [SerializeField] ParticleSystem hitFloorFx;
    [SerializeField] ParticleSystem hitWallFx;
    [SerializeField] ParticleSystem hitBallFx;
    [SerializeField] ParticleSystem successFx;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip successClip;
    [SerializeField] string floorTag = "Floor";
    [SerializeField] string wallTag = "Wall";
    [SerializeField] string ballTag = "Ball";
    [SerializeField] string targetTag = "Target";

    Rigidbody rb;
    AudioSource audioSource;
    int bounceCount;
    bool finished;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(float mass, float lifeTime)
    {
        rb.mass = mass;
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector3 direction, float impulsePower, float torquePower)
    {
        rb.AddForce(direction * impulsePower, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * torquePower, ForceMode.Impulse);
    }
    /*
    void OnCollisionEnter(Collision collision)
    {
        if (finished) return;

        bounceCount++;

        float impact = collision.relativeVelocity.magnitude;
        audioSource.PlayOneShot(hitClip, Mathf.Clamp01(impact / 15f));

        ContactPoint contact = collision.contacts[0];
        ParticleSystem fx = GetFxByTag(collision.gameObject.tag);
        if (fx != null)
            Instantiate(fx, contact.point, Quaternion.LookRotation(contact.normal));

        if (bounceCount >= 8)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (finished) return;
        if (!other.CompareTag(targetTag)) return;

        finished = true;

        if (successFx != null)
            Instantiate(successFx, transform.position, Quaternion.identity);

        if (successClip != null)
            audioSource.PlayOneShot(successClip);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    ParticleSystem GetFxByTag(string tag)
    {
        if (tag == floorTag) return hitFloorFx;
        if (tag == wallTag) return hitWallFx;
        if (tag == ballTag) return hitBallFx;
        return hitWallFx;
    }
    */
}