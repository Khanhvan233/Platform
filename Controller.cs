using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    Animator anim;

    int jumpCount = 0;
    int maxJump = 2;

    private bool isAttacking = false;
    private float comboCD = 1.2f;
    private float attackTime = 0.2f;
    private float lastAttackTime = 0f;

    bool isGrounded;
    bool wasGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Debug.Log("isGrounded: " + isGrounded);
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // nếu đang trong thời gian combo
            //if (Time.time - lastAttackTime <= comboCD)
            //{
            //    lastAttackTime = Time.time;
            //    anim.Play("Warrior_Attack2_Black");
            //    CancelInvoke(); // tránh bị reset sớm
            //    Invoke("ResetAttack", attackTime);
            //    ResetTime(); // reset thời gian combo để có thể tiếp tục chuỗi tấn công
            //}
            //else
            //{
            if(!isAttacking)
            { 
                // attack lần 1
                lastAttackTime = Time.time;
                isAttacking = true;
                anim.Play("Warrior_Attack1_Black");
                Invoke("ResetAttack", attackTime);
            }
        }

        if (!isAttacking && Mathf.Abs(move) > 0)
        {
            anim.Play("Warrior_Run_Black");
        }
    }
    //private void ResetTime()
    //{
    //    lastAttackTime = 0f;
    //}
    private void ResetAttack()
    {
        isAttacking = false;
    }
}
