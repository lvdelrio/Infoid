using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    public GameObject projectileCreator;
    private ParticleSystem particleSystem;

    
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    
    public void Launch(Vector2 initialDirection, GameObject creator)
    {
        direction = initialDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectileCreator = creator;
        InvertParticleEmission();

        // Schedule the projectile for destruction after 10 seconds
        Invoke(nameof(DestroySelf), 10f);
    }

     void InvertParticleEmission()
    {
        if (particleSystem != null)
        {
            var main = particleSystem.main;
            main.startSpeed = -main.startSpeed.constant; // Invert the start speed

            var emission = particleSystem.emission;
            var burst = emission.GetBurst(0);
            burst.count = -burst.count.constant; // Invert the burst count if applicable
            emission.SetBurst(0, burst);

            var shape = particleSystem.shape;
            shape.rotation = new Vector3(shape.rotation.x + 90, shape.rotation.y, shape.rotation.z); // Rotate the emission shape by 180 degrees
        }
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
