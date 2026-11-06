using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHp = 100;
    private int hp;

    public int Hp => hp;

    void Start()
    {
        hp = maxHp;
    }

    public void KenaDamage(int jumlah)
    {
        hp = Mathf.Max(hp - jumlah, 0);
        Debug.Log($"Player kena damage {jumlah}, HP sisa: {hp}");

        if (hp <= 0) Mati();
    }

    void Mati()
    {
        Debug.Log("Player mati!");
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<PlayerMovement>().enabled = false;
    }
}