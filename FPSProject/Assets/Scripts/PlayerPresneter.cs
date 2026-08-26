using TMPro;
using UnityEngine;

public class PlayerPresneter : MonoBehaviour
{
    [SerializeField] EnemySpawnManager spawnManager;
    [SerializeField] AmmoSystem ammo;

    [Header("UI")]
    [SerializeField] TMP_Text playerHpText;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] GameObject clearPanel;

    [SerializeField] TMP_Text curAmmo;
    [SerializeField] TMP_Text maxAmmo;

    private void OnEnable()
    {
        ammo.OnChangeAmmo += UpdateAmmo;
        ammo.OnReload += ReloadAmmo;
    }

    private void OnDisable()
    {
        ammo.OnChangeAmmo -= UpdateAmmo;
        ammo.OnReload -= ReloadAmmo;
    }

    void Start()
    {
        UpdateAmmo(ammo.CurAmmo, ammo.maxMagazine);
    }

    void UpdateAmmo(int curAm, int maxAm)
    {
        curAmmo.text = curAm.ToString();
        maxAmmo.text = maxAm.ToString();
    }

    void ReloadAmmo()
    {
        curAmmo.text = ammo.maxMagazine.ToString();
    }
}
