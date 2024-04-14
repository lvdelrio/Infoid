using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public Animator animator;
    public float jumpForce = 10f;
    public float moveSpeed = 7f;
    public float distance;
    public float FallingConst = 5f;
    
    public Rigidbody2D rb;
    public Camera camera;
    public Collider2D verticalCollider;
    private float lastYPosition;
    private float wallDirection = 0f;

    [Header("Wall Jumping System")]
    bool isTouchingWall = false;
    bool isWallSliding = false;
    private bool canMove;
    
    public float wallSlidingSpeed;
    public float wallJumpDuration;
    public float jumpAngle = 45f;
    public float wallJumpForce;

    bool wallJumping;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        verticalCollider.enabled = true;
        canMove = true;
        lastYPosition = transform.position.y;
    }

    void Update()
    {
        float moveInputx = Input.GetAxis("Horizontal");
        float moveInputy = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(moveInputx, moveInputy).normalized;

        // isTouchingWall = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.15f, 0.2f), LayerMask.GetMask("Wall"));
        if (canMove){
            if (isTouchingWall)
            {
                isWallSliding = true;
            }
            else
            {
                isWallSliding = false;   
            }

            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                transform.rotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, -7, 1);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
            } else
            {
                rb.velocity = new Vector2(moveInputx * moveSpeed, moveInputy != 0 ? moveInputy * moveSpeed : -FallingConst);

                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(7, 7, 1);
            }

            if(wallJumping)
            {
                float jumpAngleRadians = jumpAngle * Mathf.Deg2Rad;
                Vector2 jumpDirection = new Vector2(Mathf.Cos(jumpAngleRadians), Mathf.Sin(jumpAngleRadians));

                if (wallDirection < 0)
                {
                    jumpDirection.x *= -1;
                }
                // rb.velocity = new Vector2(wallJumpForce.x * wallDirection, wallJumpForce.y);
                rb.velocity = new Vector2(jumpDirection.x * wallJumpForce, jumpDirection.y * wallJumpForce);
            }


            float currentYPosition = transform.position.y;

            if (currentYPosition < lastYPosition)
            {
                distance += lastYPosition - currentYPosition;
            }

            lastYPosition = currentYPosition;

            float verticalExtent = Camera.main.orthographicSize;
            float minY = Camera.main.transform.position.y - (verticalExtent - 5);
            float maxY = Camera.main.transform.position.y + (verticalExtent - 2);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
        

    void Jump()
    {
        wallJumping = true;
        isTouchingWall = false;
        Invoke("StopWallJump", wallJumpDuration);


        // rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
        // transform.rotation = Quaternion.identity;
    }    

    void StopWallJump()
    {
        wallJumping = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            animator.SetBool("onWall", true);

            wallDirection = collision.GetContact(0).normal.x;

            if (wallDirection > 0)
            {
                Debug.Log("Left");
            }
            else if (wallDirection < 0)
            {
                Debug.Log("Right");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.rotation = Quaternion.identity;
            animator.SetBool("onWall", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag)
        {
            case "obstacle":
                Debug.Log("Player hit an obstacle!");
                break;
            case "EdgeCollider":
                Debug.Log("Player hit the edge!");
                //canMove = false;
                gameController.EndLevel();
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                //camera.GetComponent<CameraController>().ResetCameraPosition();
                //canMove = true;
                break;
            default:
                break;
        }

    }

    public void ResetPlayerPosition()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool CanMove()
    {
        return canMove;
    }
}