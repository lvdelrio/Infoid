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
    [SerializeField] int Score_reward = 200;
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
            //Crear y agregar direccion al objeto hacia el jugador
            Vector2 direction = (player.transform.position - transform.position).normalized;

            rb.AddForce(direction * speed);

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
