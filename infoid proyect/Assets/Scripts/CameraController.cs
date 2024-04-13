using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; 
    public float yOffset = 0f; 

    void Update()
    {
        if (player != null)
        {
            if (player.position.y < transform.position.y)
            {

                transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
            }
            
        }
    }
}
