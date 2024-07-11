using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeBossController : MonoBehaviour
{

    public float health = 150;
    private GameObject targetGameObject;
    [Header("particle System")]
    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector2 player)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        Vector2 knockbackDirection = ((Vector2)transform.position - player).normalized;
        //StartCoroutine(ApplyKnockback(knockbackDirection));
    }

    public void Die()
    {
        targetGameObject = GameObject.FindGameObjectWithTag("Player");
        targetGameObject.GetComponent<Level>().AddExperience(1000);
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        Debug.Log("Enemy died");
        Destroy(this.gameObject);
    }
}
