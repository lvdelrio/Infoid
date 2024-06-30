using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    [Header("Required objects")]
    public Rigidbody2D rb;
    [SerializeField] private GameController gameController;
    public GameObject shurikenPrefab;

    [Header("Enemy Attributes")]
    public float speed;
    public float maxSpeed;
    public float deathTime;
    private float Timer;
    private GameObject player;
    GameObject targetGameObject;
    private bool _hasCollideWithPlayer = false;
    private bool isFlyingAway = false;
    private Vector2 flyAwayDirection;

    [SerializeField] int experience_reward = 400;

    [Header("Boid Attributes")]
    public float neighborRadius = 2f;
    public float separationRadius = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isFlyingAway)
        {
            rb.AddForce(flyAwayDirection * speed);

            if (IsOutsideScreen())
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Vector2 boidForce = BoidBehavior();

            // Add the boid force to the enemy's movement towards the player
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.AddForce((direction * speed) + boidForce);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            }

            Timer += Time.fixedDeltaTime;

            if (Timer >= deathTime)
            {
                StartFlyingAway();
            }
        }
    }

    Vector2 BoidBehavior()
    {
        Vector2 alignment = Alignment() * alignmentWeight;
        Vector2 cohesion = Cohesion() * cohesionWeight;
        Vector2 separation = Separation() * separationWeight;

        return alignment + cohesion + separation;
    }

    Vector2 Alignment()
    {
        Vector2 steering = Vector2.zero;
        int count = 0;

        foreach (SimpleEnemyController boid in GetNeighbors())
        {
            steering += boid.rb.velocity;
            count++;
        }

        if (count > 0)
        {
            steering /= count;
            steering = steering.normalized * maxSpeed - rb.velocity;
        }

        return steering;
    }

    Vector2 Cohesion()
    {
        Vector2 steering = Vector2.zero;
        int count = 0;

        foreach (SimpleEnemyController boid in GetNeighbors())
        {
            steering += (Vector2)boid.transform.position;
            count++;
        }

        if (count > 0)
        {
            steering /= count;
            steering = steering - (Vector2)transform.position;
            steering = steering.normalized * maxSpeed - rb.velocity;
        }

        return steering;
    }

    Vector2 Separation()
    {
        Vector2 steering = Vector2.zero;
        int count = 0;

        foreach (SimpleEnemyController boid in GetNeighbors())
        {
            float distance = Vector2.Distance(transform.position, boid.transform.position);
            if (distance < separationRadius)
            {
                Vector2 diff = (Vector2)transform.position - (Vector2)boid.transform.position;
                steering += diff.normalized / distance;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count;
            steering = steering.normalized * maxSpeed - rb.velocity;
        }

        return steering;
    }

    List<SimpleEnemyController> GetNeighbors()
    {
        List<SimpleEnemyController> neighbors = new List<SimpleEnemyController>();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, neighborRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider != GetComponent<Collider2D>() && collider.GetComponent<SimpleEnemyController>())
            {
                neighbors.Add(collider.GetComponent<SimpleEnemyController>());
            }
        }

        return neighbors;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            if (gameController.RollLuck(8, 10))
            {
                Instantiate(shurikenPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    public void Die()
    {
        targetGameObject = GameObject.FindGameObjectWithTag("Player");
        targetGameObject.GetComponent<Level>().AddExperience(experience_reward);
        //add currency reward

        if (gameController.RollLuck(8, 10))
        {
            Instantiate(shurikenPrefab, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }

    public bool hasCollideWithPlayer
    {
        get { return _hasCollideWithPlayer; }
    }

    public void collidedWithPlayer()
    {
        _hasCollideWithPlayer = true;
    }

    void StartFlyingAway()
    {
        isFlyingAway = true;
        flyAwayDirection = (transform.position - player.transform.position).normalized;
        rb.velocity = Vector2.zero;
    }

    bool IsOutsideScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }
}
