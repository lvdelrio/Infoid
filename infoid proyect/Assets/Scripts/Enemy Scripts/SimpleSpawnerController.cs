using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawnerController : MonoBehaviour
{
    [Header("Enemy to Spawn")]
    public GameObject enemyPrefab;
    public float spawnRate;
    public float distanceToSpawn;
    public int maxEnemies;
    
    private float timer;

    void FixedUpdate()
    {
        if (timer >= spawnRate && GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {

            SpawnEnemy();
            timer = 0;
        }
        timer += Time.fixedDeltaTime;   

    }

    void SpawnEnemy()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 spawnPosition = new Vector2(Random.Range(-7f, 7f), playerPosition.y - distanceToSpawn);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
