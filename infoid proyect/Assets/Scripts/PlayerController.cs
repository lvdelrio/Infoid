using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private bool isWallRunning = false;

    public Collider2D verticalCollider;
    public Collider2D horizontalCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        verticalCollider.enabled = true;
        horizontalCollider.enabled = false;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallRunning)
            {
                
                rb.velocity = new Vector2(rb.velocity.x * 0.5f, jumpForce); 
                isWallRunning = false; 
            }
        }

        
        verticalCollider.enabled = !isWallRunning;
        horizontalCollider.enabled = isWallRunning;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isWallRunning = true; 
            rb.gravityScale = 0.5f; 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isWallRunning = false;
            rb.gravityScale = 1; 
        }
    }
}
