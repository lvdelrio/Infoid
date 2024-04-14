using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; 
    public float downwardSpeed = 0.5f; 

    void Update()
    {
        if (player != null && player.GetComponent<PlayerController>().CanMove())
        {
            transform.position += Vector3.down * downwardSpeed * Time.deltaTime;
            
        }
    }

    public void ResetCameraPosition()
    {
        transform.position = new Vector3(0, 0, -10);
    }
}
