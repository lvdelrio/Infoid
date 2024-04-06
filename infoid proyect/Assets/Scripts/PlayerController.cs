using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float moveSpeed = 5f;
    public float jumpAngle = 45f;
    public float slidingSpeed = 2f;
    public float wallSlideRotation = 45f;
    public Rigidbody2D rb;
    private bool isWallSliding = false;
    private bool isTouchingWall = false;
    private float wallDirection = 0f;

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
        Debug.Log("Touch: " + isTouchingWall + " Slide: " + isWallSliding);
        if (isTouchingWall)
        {
            isWallSliding = true;
            horizontalCollider.enabled = true;
            verticalCollider.enabled = false;
        }
        else
        {
            isWallSliding = false;   
        }

        
        if (isWallSliding)
        {
            rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpAngleRadians = jumpAngle * Mathf.Deg2Rad;
                Vector2 jumpDirection = new Vector2(Mathf.Cos(jumpAngleRadians), Mathf.Sin(jumpAngleRadians));

                if (wallDirection < 0)
                {
                    jumpDirection.x *= -1;
                }

                horizontalCollider.enabled = false;
                verticalCollider.enabled = true;
                
                isTouchingWall = false;
                isWallSliding = false;

                // rb.velocity = new Vector2(0f, jumpDirection.y * jumpForce);
                rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
                transform.rotation = Quaternion.identity;

                
            }
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }



        if (!isWallSliding)
        {
            horizontalCollider.enabled = false;
            verticalCollider.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            rb.gravityScale = 1f;

            wallDirection = collision.GetContact(0).normal.x;

            if (wallDirection > 0)
            {
                Debug.Log("Left");
                // transform.rotation = Quaternion.Euler(0f, 0f, -wallSlideRotation);
            }
            else if (wallDirection < 0)
            {
                Debug.Log("Right");
                // transform.rotation = Quaternion.Euler(0f, 0f, wallSlideRotation);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.gravityScale = 1.5f;
            transform.rotation = Quaternion.identity;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}