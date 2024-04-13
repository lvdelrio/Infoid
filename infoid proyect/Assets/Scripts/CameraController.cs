using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; 
    public float downwardSpeed = 0.5f; 

    void Update()
    {
        if (player != null)
        {
            transform.position += Vector3.down * downwardSpeed * Time.deltaTime;
            
        }
    }
}
