using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public GameController gameController;
    public Rigidbody2D rb;
    public Camera camera;
    public Collider2D verticalCollider;
    public Animator animator;
    public GameObject grapplingHookPrefab;
    
    
    [Header("Player Movement")]
    public float moveSpeed = 7f;
    public float distance;
    public float FallingConst = 5f;
    
    private float lastYPosition;
    private float wallDirection = 0f;
    public float grappleSpeed = 20f;
    public float grappleLifetime = 1f;

    [Header ("Stats")]
    public int health = 100;
    public int maxHealth = 100;
    public int damage = 10;
    public int luck = 1;

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

    void FixedUpdate()
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
                rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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
        
            if (Input.GetKeyDown(KeyCode.J))
            {
                FireGrapplingHook();
            }
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
                transform.rotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, 7, 1); 
                Debug.Log(transform.localScale);  
                Debug.Log("Left");
            }
            else if (wallDirection < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, -7, 1);
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
            case "Enemy":
                Debug.Log("PLayer hit enemy!");
                break;
            case "PowerUp":
                Debug.Log("Player hit power up!");
                int statToUpgrade = UnityEngine.Random.Range(0, 4);
                switch (statToUpgrade)
                {
                    case 0:
                        health += 10;
                        maxHealth += 10;
                        break;
                    case 1:
                        damage += 5;
                        break;
                    case 2:
                        luck += 1;
                        break;
                    case 3:
                        moveSpeed += 1;
                        break;
                    default:
                        break;
                }

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
    public void MoveToEnemy(Vector3 enemyPosition)
    {
        StartCoroutine(MoveTowardsPosition(enemyPosition));
    }

    IEnumerator MoveTowardsPosition(Vector3 position)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, position);
        float journeyTime = journeyLength / grappleSpeed;

        while (Time.time - startTime < journeyTime)
        {
            float fracJourney = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Lerp(startPosition, position, fracJourney);
            yield return null;
        }
    }

    void FireGrapplingHook()
    {
        float offsetDistance = 5.0f;
        Vector3 spawnPosition = transform.position + Vector3.down * offsetDistance;
        GameObject grapple = Instantiate(grapplingHookPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = grapple.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, grappleSpeed); 
        Destroy(grapple, 2f);
        Debug.Log("se destruyo en teoria");
    }
}