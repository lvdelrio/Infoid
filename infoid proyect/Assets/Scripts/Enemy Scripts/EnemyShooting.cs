using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootingRate = 2f;
    public float enemyLifetime = 5f;
    private float shootCooldown;

    void Start()
    {
        shootCooldown = 0f;
        StartCoroutine(DestroyAfterTime(enemyLifetime));
    }

    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        else
        {
            Shoot();
            shootCooldown = shootingRate;
        }
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
            projectile.GetComponent<Projectile>().Launch(directionToPlayer);
        }
        else
        {
            Debug.LogError("Player object not found. Ensure your player has the 'Player' tag.");
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
