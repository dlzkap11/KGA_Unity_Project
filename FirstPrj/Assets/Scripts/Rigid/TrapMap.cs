using UnityEngine;

public class TrapMap : MonoBehaviour
{
    /*
     *다중 감지 함정 지대
플레이어와 두 종류 이상의 함정으로 다음을 구현합니다.

함정은 가벼운 함정과 강한 함정 두 종류로 나뉘고, 각각 다른 Layer를 가진다
플레이어 정면뿐 아니라 좌/우로도 동시에 Raycast를 쏴서, 여러 각도의 함정을 한꺼번에 감지한다
각 방향의 감지 여부와 거리를 구분해서 처리하며, 가장 가까운 함정을 기준으로 경고음의 재생 간격이 거리에 비례해 빨라진다
실제로 함정에 닿으면 함정 종류에 따라 넉백 세기가 다르게 적용되고, 타격 이펙트와 소리가 함께 재생된다
강한 함정에 닿는 순간 AddExplosionForce가 발생해 주변의 가벼운 함정들까지 밀려나는 연쇄 반응이 일어난다
플레이어가 함정에 일정 횟수 이상 부딪히면 일정 시간 동안 움직일 수 없는 경직 상태가 된다
안전지대(Trigger)에 들어가면 지금까지 누적된 피격 횟수와 경직 상태가 모두 초기화된다
감지/피격 관련 모든 상태 변화(감지 시작, 피격, 경직, 초기화)가 Console에 구분되어 기록된다
 
     */
    public Rigidbody rb;
    public float KnockbackPower;
    [SerializeField]
    ParticleSystem particle;
    [SerializeField]
    GameObject particlePrefab;


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {

        if (collision.gameObject.CompareTag("Plane"))
            return;

        if (particle == null)
        {
            particle = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
            particle.transform.SetParent(transform, false);
        }
            

        rb = collision.gameObject.GetComponent<Rigidbody>();


        rb.AddExplosionForce(KnockbackPower, transform.position, 6.0f, 3.0f);
        //rb.AddExplosionForce(KnockbackPower, rb.position, 6.0f, 3.0f);
        Debug.Log($"{transform.name}에 부딫힘!");
    
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        Destroy(particle, 2.0f);
    }
}
