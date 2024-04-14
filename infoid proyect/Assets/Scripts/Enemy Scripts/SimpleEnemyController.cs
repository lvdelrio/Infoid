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
}
