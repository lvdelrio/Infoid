
using System.Collections;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public GameObject player;
    public float offSet = 5;
    public bool touchedEnemy = false;

    public void Start()
    {
        StartCoroutine(DestroyAfterTime(0.05f));
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!touchedEnemy)
        {
            transform.position = player.transform.position + new Vector3(0, -offSet, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touchedEnemy && (collision.CompareTag("Enemy") || collision.CompareTag("Perry")))
        {
            touchedEnemy = true;
            player.GetComponent<PlayerController>().MoveToEnemy(collision.transform.position, gameObject);
        }
    }

    private IEnumerator DestroyAfterTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (!touchedEnemy)
        {
            Destroy(gameObject);
        }
    }
}