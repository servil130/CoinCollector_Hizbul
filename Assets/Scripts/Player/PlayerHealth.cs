using System;                      // BARU
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHp = 100;
    private int hp;

    public int Hp => hp;
    public int MaxHp => maxHp;     // BARU

    public event Action<int, int> OnHealthChanged;   // BARU: (hp sekarang, hp maksimal)

    void Awake()                   // BARU: sebelumnya Start(), diganti supaya hp sudah terisi sebelum UI membacanya
    {
        hp = maxHp;
    }

    public void KenaDamage(int jumlah)
    {
        if (hp <= 0) return;       // BARU: abaikan damage kalau sudah mati

        hp = Mathf.Max(hp - jumlah, 0);
        Debug.Log($"Player kena damage {jumlah}, HP sisa: {hp}");

        OnHealthChanged?.Invoke(hp, maxHp);   // BARU: kabari UI

        if (hp <= 0) Mati();
    }

    void Mati()
    {
        Debug.Log("Player mati!");
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<PlayerMovement>().enabled = false;
    }
}