using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 direct;

    public int score = 0;
    public GameManager gameManager;

    // Isi sesuai Threshold tiap motion di Blend Tree
    public float blendDown = 0f;
    public float blendUp = 1f;
    public float blendLeft = 2f;
    public float blendRight = 3f;

    // Centang kalau kamu cuma punya SATU animasi samping (kanan/kiri di-flip)
    public bool useFlipForSide = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void OnMove(InputValue value)
    {
        direct = value.Get<Vector2>();
    }

    void Update()
    {
        rb.linearVelocity = direct * speed;

        bool isWalking = direct.sqrMagnitude > 0.01f;
        anim.SetBool("IsWalking", isWalking);

        if (!isWalking) return;

        if (Mathf.Abs(direct.x) > Mathf.Abs(direct.y))
        {
            if (useFlipForSide)
            {
                anim.SetFloat("Blend", blendRight);   // pakai threshold animasi samping
                sr.flipX = direct.x < 0;              // kebalik? ganti jadi direct.x > 0
            }
            else
            {
                sr.flipX = false;
                anim.SetFloat("Blend", direct.x > 0 ? blendRight : blendLeft);
            }
        }
        else
        {
            sr.flipX = false;
            anim.SetFloat("Blend", direct.y > 0 ? blendUp : blendDown);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            score++;
            Debug.Log("Score: " + score);
            gameManager.CollectCoin();
        }
    }
}