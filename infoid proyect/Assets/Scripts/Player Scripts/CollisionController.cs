using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    GameController gameController;
    public ChangeScene changeScene;
    PlayerController playerController;
    InventoryController inventoryController;
    private CameraController cameraController;
    public ItemDatabase itemDatabase;
    private bool playerInParryZone = false;
    private SimpleEnemyController currentEnemy;
    private GameObject currentEnemyGameObject;

    [Header("Audio")]
    public AudioClip obstacleHitSound;
    public AudioClip enemyHitSound;
    public AudioClip powerUpSound;
    private AudioSource audioSource;

    [HideInInspector]
    public bool skipTrigger = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameController = playerController.gameController;
        inventoryController = playerController.inventoryController;
        cameraController = Camera.main.GetComponent<CameraController>();

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (skipTrigger)
        {
            skipTrigger = false;
            return;
        }

        switch (other.tag)
        {
            case "obstacle":
                if (playerController.isOnParryBoost)
                {
                    Debug.Log("Player hit an obstacle on parry boost!");
                }
                else
                {
                    Debug.Log("Player hit an obstacle!");
                    PlaySound(obstacleHitSound);
                    playerController.StartDeathDoorCountdown();
                    cameraController.ShakeCamera();
                    playerController.StartDeathDoorCountdown();
                    playerController.ChangeMaterialOnHit();
                }
                break;

            case "EdgeCollider":
                Debug.Log("Player hit the edge!");
                gameController.EndLevel();
                playerController.ResetDeathDoorState();
                break;

            case "Enemy":
                Debug.Log("Player hit enemy!");
                PlaySound(enemyHitSound);
                currentEnemy = other.GetComponentInParent<SimpleEnemyController>();
                currentEnemy.collidedWithPlayer();
                currentEnemy.Destroy();
                cameraController.ShakeCamera();
                playerController.StartDeathDoorCountdown();
                playerController.ChangeMaterialOnHit();
                Destroy(other.gameObject);
                break;

            case "Proyectile":
                Debug.Log("Player hit projectile!");
                PlaySound(enemyHitSound);
                cameraController.ShakeCamera();
                playerController.StartDeathDoorCountdown();
                playerController.ChangeMaterialOnHit();
                break;

            case "PowerUp":
                Debug.Log("Player hit power up!");
                PlaySound(powerUpSound);
                int statToUpgrade = Random.Range(0, itemDatabase.allPassiveItems.Count);
                inventoryController.AddPassiveItem(itemDatabase.allPassiveItems[statToUpgrade]);
                break;

            case "ActiveItem":
                Debug.Log("Player hit active item!");
                inventoryController.AddActiveItem(other.GetComponent<ActiveItemPickup>().itemData);
                Destroy(other.gameObject);
                break;

            case "BlockHand":
                Debug.Log("Player hit block hand!");
                StartDeathDoorCountdown();
                
                break;

            default:
                break;
        }
    }

    private void StartDeathDoorCountdown()
    {
        StartCoroutine(playDeathSound());
    }


    private IEnumerator playDeathSound()
    {
        PlaySound(enemyHitSound);
        yield return new WaitForSeconds(0.1f);
        PlaySound(enemyHitSound);
        yield return new WaitForSeconds(0.1f);
        PlaySound(enemyHitSound);
        yield return new WaitForSeconds(0.2f);
        PlaySound(enemyHitSound);
        yield return new WaitForSeconds(0.1f);
        PlaySound(enemyHitSound);
        changeScene.MoveToScene(0);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound clip or AudioSource is missing!");
        }
    }
}