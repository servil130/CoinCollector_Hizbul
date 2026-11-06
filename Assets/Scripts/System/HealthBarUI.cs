using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthFill;     // bar health (depan)
    [SerializeField] private Image depletedFill;   // bar depleted (tengah, menyusul turun)

    [SerializeField] private float depletedDelay = 0.4f;   // jeda sebelum bar depleted turun
    [SerializeField] private float depletedSpeed = 0.5f;   // kecepatan turun (per detik)

    private float depletedTimer;

    void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    void Start()
    {
        if (playerHealth == null) return;

        float ratio = (float)playerHealth.Hp / playerHealth.MaxHp;
        healthFill.fillAmount = ratio;
        depletedFill.fillAmount = ratio;
    }

    void HandleHealthChanged(int current, int max)
    {
        float ratio = (float)current / max;
        healthFill.fillAmount = ratio;

        // Kalau HP naik (heal), bar depleted ikut langsung. Kalau turun, tunggu dulu.
        if (depletedFill.fillAmount < ratio)
            depletedFill.fillAmount = ratio;
        else
            depletedTimer = depletedDelay;
    }

    void Update()
    {
        if (depletedFill.fillAmount <= healthFill.fillAmount) return;

        if (depletedTimer > 0f)
        {
            depletedTimer -= Time.deltaTime;
            return;
        }

        depletedFill.fillAmount = Mathf.MoveTowards(
            depletedFill.fillAmount,
            healthFill.fillAmount,
            depletedSpeed * Time.deltaTime
        );
    }
}