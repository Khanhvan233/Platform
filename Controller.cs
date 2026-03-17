using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    Animation anim;

    int jumpCount = 0;
    int maxJump = 2;

    bool isGrounded;
    bool wasGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // chỉ reset khi vừa chạm đất
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }

        // di chuyển 
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);


        // xoay mặt nhân vật
        if (move > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }

    }
}