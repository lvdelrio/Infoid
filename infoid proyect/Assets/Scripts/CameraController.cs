using UnityEngine;

public class CameraController : MonoBehaviour
{
public GameObject player;
public float downwardSpeed = 5f;
public float catchUpMultiplier = 1.2f; 
public float speedUp = 1.0f; 

void FixedUpdate()
{
    if (player != null && player.GetComponent<PlayerController>().CanMove())
    {
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            float playerSpeed = playerRigidbody.velocity.y;

            float cameraBottom = transform.position.y - Camera.main.orthographicSize;

            if (player.transform.position.y < cameraBottom + speedUp || playerSpeed < -downwardSpeed)
            {
                float adjustedSpeed = Mathf.Max(downwardSpeed, Mathf.Abs(playerSpeed) / catchUpMultiplier);
                transform.position += Vector3.down * adjustedSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.down * downwardSpeed * Time.deltaTime;
            }
        }
    }
}



    public void ResetCameraPosition()
    {
        transform.position = new Vector3(0, 0, -10);
    }
}