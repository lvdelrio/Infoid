using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    public GameObject projectileCreator;

    public void Launch(Vector2 initialDirection, GameObject creator)
    {
        direction = initialDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectileCreator = creator;

        // Schedule the projectile for destruction after 10 seconds
        Invoke(nameof(DestroySelf), 10f);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!");
            Destroy(gameObject);
        }
        else if (other.gameObject != projectileCreator)
        {
            Destroy(gameObject);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
