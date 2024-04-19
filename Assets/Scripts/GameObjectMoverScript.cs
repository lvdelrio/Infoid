// Encargado de mover las muralla para arriba y eliminar la muralla cuando ya esta fuera de la pantalla

using UnityEngine;

public class GameObjectMoverScript : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }


    void Update()
    {
        if (!GameManagerScript.Instance.lost)
        {
            // Movemos el GameObject para arriba en cada frame
            transform.position += Vector3.up * Config.FALL_SPEED * Time.deltaTime;

            if (!IsInView())
            {
                Destroy(gameObject);
            }
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
