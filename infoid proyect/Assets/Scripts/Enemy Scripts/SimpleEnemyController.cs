using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    [Header("Required objects")]
    public Rigidbody2D rb;

    [Header("Enemy Attributes")]
    public float speed;
    public float maxSpeed;
    public float deathTime;
    private float Timer;
    private GameObject player;
    GameObject targetGameObject;
    private bool _hasCollideWithPlayer = false;

    [SerializeField] int experience_reward = 400;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
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
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }
    public void Die()
    {
        targetGameObject = GameObject.FindGameObjectWithTag("Player");
        targetGameObject.GetComponent<Level>().AddExperience(experience_reward);
        Debug.Log("Enemy died");
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


}
