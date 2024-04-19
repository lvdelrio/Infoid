// Encargado de controlar el enemigo bat

using UnityEngine;

public class BatScript : MonoBehaviour
{
    Camera mainCamera;
    GameObject player;
    private Vector3 targetPosition;
    private bool moving = true;

    void Start()
    {
        player = GameObject.Find("Player");
        mainCamera = Camera.main;
        targetPosition = player.transform.position;
        targetPosition.y = targetPosition.y + (2f * mainCamera.orthographicSize);
    }

    void FixedUpdate()
    {
        if (!GameManagerScript.Instance.lost && moving)
        {
            float step = Config.ENEMY_SPEED * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                moving = false;
            }
        }
    }

    void Update()
    {
        if (!IsInView())
        {
            Destroy(gameObject);
        }
    }


    private bool IsInView()
    {
        float height = transform.localScale.y;
        float topEdgePosition = transform.position.y + height / 2.0f;
        float bottomEdgePosition = transform.position.y - height / 2.0f;

        Vector3 topEdgeScreenPoint = mainCamera.WorldToViewportPoint(new Vector3(transform.position.x, topEdgePosition, transform.position.z));
        Vector3 bottomEdgeScreenPoint = mainCamera.WorldToViewportPoint(new Vector3(transform.position.x, bottomEdgePosition, transform.position.z));

        return topEdgeScreenPoint.y >= 0 && bottomEdgeScreenPoint.y <= 1;
    }
}
