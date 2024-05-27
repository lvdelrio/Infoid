using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerController : MonoBehaviour
{
    [Header("Item to Spawn")]
    public GameObject itemPickupPrefab;
    public float spawnRate;
    public float distanceToSpawn;
    public int maxItems;

    private List<GameObject> _items = new List<GameObject>();
    private float _timer;

    void FixedUpdate()
    {
        if (_timer >= spawnRate && _items.Count < maxItems)
        {
            SpawnItem();
            _timer = 0;
        }
        _timer += Time.fixedDeltaTime;
    }

    void SpawnItem()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 spawnPosition = new Vector2(Random.Range(-7f, 7f), playerPosition.y - distanceToSpawn);
        GameObject newItem = Instantiate(itemPickupPrefab, spawnPosition, Quaternion.identity);
        _items.Add(newItem);
    }

    public void DestroyItem(GameObject item)
    {
        _items.Remove(item);
        Destroy(item);
    }

    public void DestroyAllItems()
    {
        foreach (GameObject item in _items)
        {
            Destroy(item);
        }
        _items.Clear();
    }
}
