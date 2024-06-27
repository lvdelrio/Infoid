using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    GameController gameController;
    PlayerController playerController;
    InventoryController inventoryController;
    private CameraController cameraController;
    public ItemDatabase itemDatabase;
    private bool playerInParryZone = false;
    private SimpleEnemyController currentEnemy;
    private GameObject currentEnemyGameObject;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameController = playerController.gameController;
        inventoryController = playerController.inventoryController;
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame

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
                    //enter Deathdoor
                    playerController.StartDeathDoorCountdown();

                }

                break;

            case "EdgeCollider":
                Debug.Log("Player hit the edge!");
                //canMove = false;
                gameController.EndLevel();
                playerController.ResetDeathDoorState();
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                //camera.GetComponent<CameraController>().ResetCameraPosition();
                //canMove = true;
                break;

            case "Enemy":
                Debug.Log("Player hit enemy!");
                currentEnemy = other.GetComponentInParent<SimpleEnemyController>();
                currentEnemy.collidedWithPlayer();
                cameraController.ShakeCamera();
                //enter Deathdoor
                playerController.StartDeathDoorCountdown();

                break;

            case "PowerUp":
                Debug.Log("Player hit power up!");
                Debug.Log(itemDatabase.allPassiveItems.Count);
                int statToUpgrade = UnityEngine.Random.Range(0, itemDatabase.allPassiveItems.Count);
                Debug.Log(statToUpgrade);
                inventoryController.AddPassiveItem(itemDatabase.allPassiveItems[statToUpgrade]);
                break;
            case "BlockHand":
                gameController.finish();
                break;

            default:
                break;
        }
    }

}
