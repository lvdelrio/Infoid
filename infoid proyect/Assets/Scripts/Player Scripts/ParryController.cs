using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public PlayerController playerController;
    public List<SimpleEnemyController> enemies = new List<SimpleEnemyController>();
    public float colliderRadius;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            SimpleEnemyController enemy = other.GetComponent<SimpleEnemyController>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
    }

    public void Parry()
    {
        foreach (SimpleEnemyController enemy in enemies)
        {
            enemy.Die();
            StartCoroutine(playerController.parryBoost());
        }
        enemies.Clear();
    }

    void Update()
    {
        // Create a list to store enemies that need to be removed
        List<SimpleEnemyController> enemiesToRemove = new List<SimpleEnemyController>();

        foreach (SimpleEnemyController enemy in enemies)
        {
            Debug.Log(Vector3.Distance(transform.position, enemy.transform.position));
            if (Vector3.Distance(transform.position, enemy.transform.position) > colliderRadius)
            {
                enemiesToRemove.Add(enemy);
            }
        }

        // Remove enemies outside the main loop
        foreach (SimpleEnemyController enemy in enemiesToRemove)
        {
            enemies.Remove(enemy);
        }
    }
}
