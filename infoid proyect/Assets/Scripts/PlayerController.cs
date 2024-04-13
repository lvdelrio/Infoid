using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float jumpForce = 10f;
    public float moveSpeed = 7f;
    public float distance;
    public float FallingCosnt = 5f;
    public float jumpAngle = 45f;
    public float slidingSpeed = 2f;
    public float wallSlideRotation = 45f;
    public Rigidbody2D rb;
    public Camera camera;
    private bool isWallSliding = false;
    private bool isTouchingWall = false;
    private float wallDirection = 0f;

    public Collider2D verticalCollider;
    public Collider2D horizontalCollider;
    private float lastYPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        verticalCollider.enabled = true;
        horizontalCollider.enabled = false;
        lastYPosition = transform.position.y;
    }

    void Update()
    {

        float moveInputx = Input.GetAxis("Horizontal");
        float moveInputy = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(moveInputx, moveInputy).normalized;
        //Debug.Log("Touch: " + isTouchingWall + " Slide: " + isWallSliding);
        if (isTouchingWall)
        {
            isWallSliding = true;
            //horizontalCollider.enabled = true;
            //verticalCollider.enabled = false;
        }
        else
        {
            isWallSliding = false;   
        }

        
        if (isTouchingWall)
        {
            rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
            transform.rotation = Quaternion.Euler(0, 0, -90);
            transform.localScale = new Vector3(7, -7, 1);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpAngleRadians = jumpAngle * Mathf.Deg2Rad;
                Vector2 jumpDirection = new Vector2(Mathf.Cos(jumpAngleRadians), Mathf.Sin(jumpAngleRadians));

                if (wallDirection < 0)
                {
                    jumpDirection.x *= -1;
                }

                //horizontalCollider.enabled = false;
                //verticalCollider.enabled = true;
                
                isTouchingWall = false;
                isWallSliding = false;

                // rb.velocity = new Vector2(0f, jumpDirection.y * jumpForce);
                //rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
                //transform.rotation = Quaternion.identity;

                
            }
        }
        else
        {
            rb.velocity = new Vector2(moveInputx * moveSpeed, moveInputy != 0 ? moveInputy * moveSpeed : -FallingCosnt);

            //Debug.Log(moveInputy + "y and speed" + moveSpeed);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(7, 7, 1);
        }

        float currentYPosition = transform.position.y;

        // Check if the player has moved downward
        if (currentYPosition < lastYPosition)
        {
            // Add only the downward distance
            distance += lastYPosition - currentYPosition;
        }

        // Update lastYPosition to the current position at the end of the frame
        lastYPosition = currentYPosition;

        /* if (!isWallSliding)
        {
            horizontalCollider.enabled = false;
            verticalCollider.enabled = true;
        } */
         float verticalExtent = Camera.main.orthographicSize; // Half the height of the camera view
        //Debug.Log("soy vertical"+verticalExtent);
        float minY = Camera.main.transform.position.y - (verticalExtent-5);
        float maxY = Camera.main.transform.position.y + (verticalExtent-2);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            animator.SetBool("onWall",true);
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
            animator.SetBool("onWall",false);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}