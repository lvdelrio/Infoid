
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public GameObject player;
    private bool hasInteracted = false;
    public float offSet = 5;

    private void start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update(){
        Vector3 newPosition = transform.position;
        newPosition.y = player.transform.position.y + offSet;
        transform.position = newPosition;
        Debug.Log("ola " + transform.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasInteracted || !collision.CompareTag("Enemy"))
        {
            return;
        }
        hasInteracted = true;
        
        if (player != null)
        {
            Debug.Log("voy al enemmigo");
            //playerController.MoveToEnemy(collision.transform.position);
        }
        Destroy(collision.gameObject);
    }
}