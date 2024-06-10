using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Required objects")]
    public Rigidbody2D rb;

    [Header("Enemy Attributes")]
    public int health;
    public float speed;
    public float maxSpeed;
    public float deathTime;
    private float Timer;
    private GameObject player;
    private GameObject targetGameObject;
    private bool isMovingSideToSide;
    private Vector2 sideToSideDirection;
    public float stopDistance;  // Distance at which the enemy stops moving downwards
    public float resumeDistance;  // Distance at which the enemy resumes moving downwards

    private bool isStopped;

    [Header("Boundary Constraints")]
    public float minX;
    public float maxX;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = player.GetComponent<PlayerController>().moveSpeed * 0.85f;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SideToSideMovement());
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
        MoveDown();
    }

    void MoveDown()
    {
        float playerY = player.transform.position.y;
        float enemyY = transform.position.y;
        float distanceToPlayer = playerY - enemyY;

        if (distanceToPlayer < 0)
        {
            float currentSpeed = speed * 2;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, -currentSpeed);
        }

        if (distanceToPlayer > stopDistance)
        {
            isStopped = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else if (distanceToPlayer < resumeDistance)
        {
            isStopped = false;
        }
    }

    IEnumerator SideToSideMovement()
    {
        while (true)
        {
            // Randomize the side-to-side direction and duration
            float duration = Random.Range(0.1f, 0.5f);
            sideToSideDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;
            isMovingSideToSide = true;
            yield return new WaitForSeconds(duration);

            isMovingSideToSide = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }

    void Update()
    {
        if (isMovingSideToSide)
        {
            rb.velocity = new Vector2(sideToSideDirection.x * speed, rb.velocity.y);
        }

        // Clamp the enemy's position to the specified x bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
    }

    public void Die()
    {
        targetGameObject = GameObject.FindGameObjectWithTag("Player");
        targetGameObject.GetComponent<Level>().AddExperience(1000);
        Debug.Log("Enemy died");
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
