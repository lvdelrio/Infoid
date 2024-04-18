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
    
    private List<GameObject> _enemies = new List<GameObject>();
    private float _timer;

    void FixedUpdate()
    {
        if (_timer >= spawnRate && GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {

            SpawnEnemy();
            _timer = 0;
        }
        _timer += Time.fixedDeltaTime;   

    }

    void SpawnEnemy()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 spawnPosition = new Vector2(Random.Range(-7f, 7f), playerPosition.y - distanceToSpawn);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        _enemies.Add(newEnemy);
    }

    public void DestroyEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy);
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in _enemies)
        {
            Destroy(enemy);
        }
        _enemies.Clear();
    }
}
