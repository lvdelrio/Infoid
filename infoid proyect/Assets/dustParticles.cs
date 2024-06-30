using UnityEngine;

public class PlayerVelocityParticles : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private PlayerController playerController;
    public float particleSpeedMultiplier = 0.5f;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        var main = particleSystem.main;
        var emission = particleSystem.emission;

        // Get player's vertical velocity
        float playerVelocity = playerController.GetComponent<Rigidbody2D>().velocity.y;

        // Only emit particles when moving down
        if (playerVelocity < 0)
        {
            emission.enabled = true;
            main.startSpeed = -playerVelocity * particleSpeedMultiplier;
        }
        else
        {
            emission.enabled = false;
        }
    }
}