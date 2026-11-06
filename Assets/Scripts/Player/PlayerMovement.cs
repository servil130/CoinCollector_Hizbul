using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Harus sama dengan Threshold di Blend Tree Move dan Idle
    private const float BlendDown = 0f;
    private const float BlendLeft = 1f / 3f;
    private const float BlendRight = 2f / 3f;
    private const float BlendUp = 1f;

    private float speed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direct;

    public int score = 0;
    public GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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

        float blend;
        if (Mathf.Abs(direct.x) > Mathf.Abs(direct.y))
            blend = direct.x > 0 ? BlendRight : BlendLeft;
        else
            blend = direct.y > 0 ? BlendUp : BlendDown;

        anim.SetFloat("Blend", blend);
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