using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 100;
    public float ms = 2f;
    protected Transform player;
    [SerializeField] private float jarakDeteksi = 6f;
    [SerializeField] private float jarakSerang = 1.2f;

    [Header("Ledakan")]
    [SerializeField] private float waktuSumbu = 1.5f;
    [SerializeField] private float radiusLedakan = 2f;
    [SerializeField] private int damageLedakan = 20;

    private StateZpmbie currentState = StateZpmbie.Idle;
    private float sumbuMulai = -1f;
    private SpriteRenderer sr;
    private Color warnaAsli;

    public TMP_Text status;

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) warnaAsli = sr.color;
    }

    void Update()
    {
        PeriksaTransisi();

        // Player menjauh sebelum sumbu habis: batalkan ledakan
        if (currentState != StateZpmbie.Attack) ResetSumbu();

        switch (currentState)
        {
            case StateZpmbie.Idle:
                PerilakuIdle();
                break;
            case StateZpmbie.Patrol:
                PerilakuPatrol();
                break;
            case StateZpmbie.Chase:
                PerilakuChase();
                break;
            case StateZpmbie.Attack:
                PerilakuAttack();
                break;
        }
    }

    public void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
        Meledak();
    }

    protected void Meledak()
    {
        if (player != null && JarakPlayer() <= radiusLedakan)
        {
            IDamageable target = player.GetComponentInParent<IDamageable>();
            if (target != null) target.KenaDamage(damageLedakan);
        }

        Mati();
    }

    public void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {hp}");

        if (hp <= 0)
        {
            Mati();
        }
    }

    protected void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");
        if (status != null) status.text = "Enemy mati";
        Destroy(gameObject);
    }

    void PerilakuIdle()
    {
        if (status != null) status.text = "Enemy idle";
    }

    void PerilakuPatrol()
    {
        if (status != null) status.text = "Enemy patrol";
    }

    void PerilakuChase()
    {
        Kejar();
        if (status != null) status.text = "Enemy ngejar";
    }

    void PerilakuAttack()
    {
        if (status != null) status.text = "Enemy mau meledak";

        if (sumbuMulai < 0f) sumbuMulai = Time.time;

        // Kedip merah selama sumbu menyala
        if (sr != null)
            sr.color = Color.Lerp(warnaAsli, Color.red, Mathf.PingPong(Time.time * 6f, 1f));

        if (Time.time >= sumbuMulai + waktuSumbu) Serang();
    }

    void ResetSumbu()
    {
        if (sumbuMulai < 0f) return;

        sumbuMulai = -1f;
        if (sr != null) sr.color = warnaAsli;
    }

    public float JarakPlayer()
    {
        if (player == null) return Mathf.Infinity;

        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakPlayer();

        if (jarak <= jarakSerang)
        {
            currentState = StateZpmbie.Attack;
        }
        else if (jarak <= jarakDeteksi)
        {
            currentState = StateZpmbie.Chase;
        }
        else
        {
            currentState = StateZpmbie.Patrol;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusLedakan);
    }
}