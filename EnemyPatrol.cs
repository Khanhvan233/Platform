using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed;
    public bool moveRight;
    private Rigidbody2D rb;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask wallLayer;
    private bool hittingWall;

    private bool atEdge;
    public Transform edgeCheck;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EnemyPatrol cần Rigidbody2D trên object!");
        }
    }

    void FixedUpdate() // dùng FixedUpdate thay vì Update
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
        atEdge = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, wallLayer);

        if (hittingWall || !atEdge)
        {
            moveRight = !moveRight; // Đảo hướng khi chạm tường
        }   

        if (rb == null) return;

        if (moveRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else if (!moveRight)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        // Đảo hướng sprite
        transform.localScale = new Vector3(moveRight ? 1 : -1, 1, 1);
    }
}