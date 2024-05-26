using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public GameController gameController;
    public InventoryController inventoryController;
    public Rigidbody2D rb;
    public Camera camera;
    public Animator animator;
    public GameObject grapplingHookPrefab;


    [Header("Player Movement")]
    public float moveSpeed = 10f;
    private float DefaultMoveSpeed = 10f;
    private float moveSpeedBoost = 2f;
    private float boostDuration = 3f;
    public float distance;
    public float FallingConst = 5f;

    private float lastYPosition;
    private float wallDirection = 0f;
    public float grappleSpeed = 20f;
    public float grappleLifetime = 1f;

    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;
    public int damage = 10;
    public int luck = 1;

    [Header("Wall Jumping System")]
    bool isTouchingWall = false;
    bool isWallSliding = false;
    private bool canMove = true;
    public float wallSlidingSpeed;
    public float wallJumpDuration;
    public float jumpAngle = 45f;
    public float wallJumpForce;
    bool wallJumping;
    private bool wantsToJump = false;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastYPosition = transform.position.y;
    }

    void Update()
    {
        if (isTouchingWall && Input.GetKeyDown(KeyCode.Space))
        {
            wantsToJump = true;
        }
    }

    void FixedUpdate()
    {
        float moveInputx = Input.GetAxis("Horizontal");
        float moveInputy = Input.GetAxis("Vertical");

        if (canMove)
        {
            if (isTouchingWall)
            {
                isWallSliding = true;

            }
            else
            {
                isWallSliding = false;
            }

            if (wallJumping)
            {
                float jumpAngleRadians = jumpAngle * Mathf.Deg2Rad;
                Vector2 jumpDirection = new Vector2(Mathf.Cos(jumpAngleRadians), Mathf.Sin(jumpAngleRadians));

                jumpDirection.x = Mathf.Abs(jumpDirection.x) * wallDirection;
                rb.velocity = new Vector2(jumpDirection.x * wallJumpForce, jumpDirection.y * wallJumpForce);
            }
            else if (isWallSliding)
            {
                rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

                if (wantsToJump)
                {
                    Jump();
                }
            }
            else
            {
                rb.velocity = new Vector2(moveInputx * moveSpeed, moveInputy != 0 ? moveInputy * moveSpeed : -FallingConst);

                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(7, 7, 1);
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }  
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy was hit");
            enemy.GetComponent<SimpleEnemyController>().Die();
            StartCoroutine(boost());
        }
    }
    IEnumerator boost()
    {
        moveSpeed += moveSpeedBoost;
        yield return new WaitForSeconds(boostDuration);
        moveSpeed = DefaultMoveSpeed;
    }
    public void IncreaseStats(StatBonus statBonus)
    {
        this.moveSpeed += statBonus.moveSpeedBonus;
        this.maxHealth += statBonus.maxHealthBonus;
        this.damage += statBonus.damageBonus;
        this.luck += statBonus.luckBonus;
    }


    public void DecreaseStats(StatBonus statBonus)
    {
        this.moveSpeed -= statBonus.moveSpeedBonus;
        this.maxHealth -= statBonus.maxHealthBonus;
        this.damage -= statBonus.damageBonus;
        this.luck -= statBonus.luckBonus;
    }


    void Jump()
    {
        wallJumping = true;
        isTouchingWall = false;
        wantsToJump = false;

        Invoke("StopWallJump", wallJumpDuration);

        // rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
        // transform.rotation = Quaternion.identity;
    }

    void StopWallJump()
    {
        wallJumping = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = new Vector3(7, 7, 1);
        animator.SetBool("onWall", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !wallJumping)
        {
            isTouchingWall = true;
            animator.SetBool("onWall", true);

            wallDirection = collision.GetContact(0).normal.x;

            if (transform.localPosition.x < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, 7, 1);
                transform.localPosition = new Vector3(-9.48f, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.localPosition.x > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, -7, 1);
                transform.localPosition = new Vector3(9.48f, transform.localPosition.y, transform.localPosition.z);
            }
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
        Debug.Log("disparo y destruyendo");
        Destroy(grapple, 2f);
        Debug.Log("se destruyo en teoria");
    }
}
