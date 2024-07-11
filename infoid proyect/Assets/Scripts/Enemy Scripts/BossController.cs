using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Required objects")]
    public Rigidbody2D rb;
    public GameObject projectilePrefab;

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
    private Coroutine shootingCoroutine;

    [Header("Boundary Constraints")]
    public float minX;
    public float maxX;

    private DecisionTreeNode decisionTree;
    public float knockbackForce = 100f;
    public float knockbackDuration = 0.5f;
    private bool isKnockedBack = false;
    [Header("particle system")]
    public ParticleSystem particleSystem;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = player.GetComponent<PlayerController>().moveSpeed * 0.85f;
        rb = GetComponent<Rigidbody2D>();
        BuildDecisionTree();
        StartCoroutine(SideToSideMovement());
    }

    void FixedUpdate()
    {
        if(!isKnockedBack){
            decisionTree.Execute(this);
        }
    }
    void Update()
    {
        if(!isKnockedBack){
            if (isMovingSideToSide)
            {
                rb.velocity = new Vector2(sideToSideDirection.x * speed, rb.velocity.y);
            }

            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            transform.position = clampedPosition;

        }
    }

    void BuildDecisionTree()
    {
        decisionTree = new DecisionNode(
            boss => boss.IsAbovePlayer(),
            new ActionNode(boss => boss.MoveDownFast()),
            new DecisionNode(
                boss => boss.IsFarFromPlayer(),
                new ActionNode(boss => boss.StopAndShootContinuously()),
                new ActionNode(boss => boss.MoveDown())
            )
        );
    }

    public bool IsAbovePlayer()
    {
        return transform.position.y > player.transform.position.y;
    }

    public bool IsFarFromPlayer()
    {
        float distanceToPlayer = player.transform.position.y - transform.position.y;
        return distanceToPlayer > stopDistance;
    }

    public void MoveDownFast()
    {
        if(!isKnockedBack){
            rb.velocity = new Vector2(rb.velocity.x, -speed * 2);
        }
    }

    public void MoveDown()
    {
        if (!isKnockedBack){
            if (!isStopped)
            {
                rb.velocity = new Vector2(rb.velocity.x, -speed);
            }

        }
    }

    public void StopMoving()
    {
        isStopped = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    public void StopAndShootContinuously()
    {
        StopMoving();
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootContinuously());
        }
    }

    IEnumerator ShootContinuously()
    {
        while (isStopped)
        {
            Shoot();
            yield return new WaitForSeconds(1f);
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


    public void Die()
    {
        targetGameObject = GameObject.FindGameObjectWithTag("Player");
        targetGameObject.GetComponent<Level>().AddExperience(1000);
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        Debug.Log("Enemy died");
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage, Vector2 player)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
       Vector2 knockbackDirection = ((Vector2)transform.position - player).normalized;
        StartCoroutine(ApplyKnockback(knockbackDirection));
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero; // Resetea la velocidad actual
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        
        Debug.DrawRay(transform.position, knockbackDirection * knockbackForce, Color.red, knockbackDuration);
        Debug.Log($"Knockback aplicado: {knockbackDirection * knockbackForce}");

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero; // Resetea la velocidad después del knockback
        isKnockedBack = false;
    }
    private void Shoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerPosition = player.transform.position;
            Vector2 enemyPosition = transform.position;
            Vector2 directionToPlayer = (playerPosition - enemyPosition).normalized;
            
            var projectile = Instantiate(projectilePrefab, enemyPosition, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Launch(directionToPlayer, gameObject);
        }
        else
        {
            Debug.LogError("Player object not found. Ensure your player has the 'Player' tag.");
        }
    }
}
