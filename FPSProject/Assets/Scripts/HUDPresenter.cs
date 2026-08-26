using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPresenter : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private Health health;


    [Header("View")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text curHp;
    [SerializeField] private TMP_Text maxHp;


    private void OnEnable()
    {
        health.OnDamaged += UpdateHp;
    }

    private void OnDisable()
    {
        health.OnDamaged -= UpdateHp;
    }

    private void Start()
    {
        UpdateHp(health.currentHealth, health.maxHealth);
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void UpdateHp(int curHp, int maxHp)
    {
        
        slider.maxValue = maxHp;
        slider.value = curHp;
        this.curHp.text = curHp.ToString();
        this.maxHp.text = maxHp.ToString();
    }

}
