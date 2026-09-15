using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 100;
    public float ms = 2f;
    protected Transform player;
    [SerializeField] private float jarakDeteksi = 6f;
    [SerializeField] private float jarakSerang = 1.2f;
    [SerializeField] private float jedaSerang= 1f;

    private StateZpmbie currentState = StateZpmbie.Idle;
    private float lastAttackTime;
    public TMP_Text status;
    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        PeriksaTransisi();
        switch(currentState)
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
        Debug.Log("Enemy menyerang!");
        
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

    private void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");
        if (status != null) status.text = "Enemy mati";
        Destroy(gameObject);
    }
    void PerilakuIdle()
    {
        Debug.Log("Enemy dalam keadaan Idle");
        if (status != null) status.text = "Enemy idle";
    }
    void PerilakuPatrol()
    {
        Debug.Log("Enemy dalam keadaan Patrol");
        if (status != null) status.text = "Enemy patrol";
    }
    void PerilakuChase()
    {
        Kejar();
        Debug.Log("Enemy dalam keadaan Chase");
        if (status != null) status.text = "Enemy ngejar";
    }
    void PerilakuAttack()
    {
        Debug.Log("Enemy dalam keadaan Attack");
        if (status != null) status.text = "Enemy menyerang";
    }
    public float JarakPlayer()
    {
        if (player == null) return Mathf.Infinity;

        return Vector2.Distance(transform.position, player.position);
    }
    void PeriksaTransisi()
    {
        float jarak = JarakPlayer();

    if  (jarak <= jarakSerang)
    {
        currentState = StateZpmbie.Attack;
    }else if (jarak <= jarakDeteksi)
    {
        currentState = StateZpmbie.Chase;
    }
    else
    {
        currentState = StateZpmbie.Patrol;
    }
    }
}
