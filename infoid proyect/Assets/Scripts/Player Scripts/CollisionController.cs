using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    GameController gameController;
    PlayerController playerController;
    InventoryController inventoryController;
    public ItemDatabase itemDatabase;
    private bool playerInParryZone = false;
    private SimpleEnemyController currentEnemy;
    private GameObject currentEnemyGameObject;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameController = playerController.gameController;
        inventoryController = playerController.inventoryController;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInParryZone && Input.GetKeyDown(KeyCode.K) && currentEnemyGameObject && currentEnemy && !currentEnemy.hasCollideWithPlayer)
        {
            Destroy(currentEnemyGameObject);
            playerController.onParry();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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

                }

                break;

            case "EdgeCollider":
                Debug.Log("Player hit the edge!");
                //canMove = false;
                gameController.EndLevel();
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                //camera.GetComponent<CameraController>().ResetCameraPosition();
                //canMove = true;
                break;

            case "Enemy":
                Debug.Log("Player hit enemy!");
                currentEnemy = other.GetComponentInParent<SimpleEnemyController>();
                currentEnemy.collidedWithPlayer();

                break;

            case "Parry":
                playerInParryZone = true;
                currentEnemy = other.GetComponentInParent<SimpleEnemyController>();
                currentEnemyGameObject = other.transform.parent.gameObject;

                break;

            case "PowerUp":
                Debug.Log("Player hit power up!");
                Debug.Log(itemDatabase.allPassiveItems.Count);
                int statToUpgrade = UnityEngine.Random.Range(0, itemDatabase.allPassiveItems.Count);
                Debug.Log(statToUpgrade);
                inventoryController.AddPassiveItem(itemDatabase.allPassiveItems[statToUpgrade]);
                break;

            default:
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Parry"))
        {
            playerInParryZone = false;
        }
    }
}
