
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public GameObject player;
    private bool hasInteracted = false;
    public float offSet = 5;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update(){
        transform.position = player.transform.position + new Vector3(0, -offSet, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("voy al enemmigo");
            player.GetComponent<PlayerController>().MoveToEnemy(collision.transform.position);
        }
    }
}