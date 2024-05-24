using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    GameController gameController;
    PlayerController playerController;
    InventoryController inventoryController;
    public ItemDatabase itemDatabase;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameController = playerController.gameController;
        inventoryController = playerController.inventoryController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "obstacle":
                Debug.Log("Player hit an obstacle!");
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
                Debug.Log("PLayer hit enemy!");
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
}
