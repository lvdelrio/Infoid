using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform player; 
    public float yOffset = 0f; 

    void Update()
    {
        if (player != null)
        {
            
            transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
        }
    }
}
