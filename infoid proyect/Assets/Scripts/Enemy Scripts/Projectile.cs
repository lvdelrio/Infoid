using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;

    public void Launch(Vector2 initialDirection)
    {
        direction = initialDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        else if (other.gameObject != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
