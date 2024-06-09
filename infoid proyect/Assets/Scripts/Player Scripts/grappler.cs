
using System.Collections;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public GameObject player;
    public float offSet = 5;
    public bool touchedEnemy = false;
    public GameObject hookPrefab;

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
        if (!touchedEnemy && collision.CompareTag("Enemy"))
        {
            touchedEnemy = true;

            Vector3 endPosition = collision.transform.position;
            Vector3 startPosition = player.transform.position;

            GameObject finalHook = Instantiate(hookPrefab, startPosition, Quaternion.identity);

            Vector3 direction = (endPosition - startPosition).normalized;
            float distance = Vector3.Distance(startPosition, endPosition);

            finalHook.transform.localScale = new Vector3(distance, 1f, finalHook.transform.localScale.z);
            finalHook.transform.position = startPosition + direction * distance / 2;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            finalHook.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


            Destroy(collision.gameObject);
            player.GetComponent<PlayerController>().MoveToEnemy(collision.transform.position, finalHook);
            Destroy(gameObject);
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