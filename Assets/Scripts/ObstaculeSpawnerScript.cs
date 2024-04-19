// Encargado de crear nuevos obstaculos

using UnityEngine;

public class ObstaculeSpawnerScript : MonoBehaviour
{
    public Camera mainCamera;
    private float screenHeight;
    private float screenWidth;
    private GameObject obstaclePrefab;
    private GameObject obstacle;

    void Start()
    {
        screenHeight = 2f * mainCamera.orthographicSize;
        screenWidth = screenHeight * mainCamera.aspect;
        obstaclePrefab = Resources.Load<GameObject>("Prefabs/Obstacle");
    }

    void Update()
    {
        // No es necesario ver en que parte de la pantalla esta ya que el GameObjectMoverScript lo elimina cuando sale de la pantalla
        if (!Config.DEBUG && obstacle == null && Random.Range(0, 3) == 0)
        {
            obstacle = Instantiate(obstaclePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
            float obstacleHeight = obstacle.transform.localScale.y;
            float startYPosition = cameraBottom - obstacleHeight / 2.0f + 0.1f;
            float randomX = Random.Range(screenWidth / 2 * -1 + obstacle.transform.localScale.x, screenWidth / 2 - obstacle.transform.localScale.x);

            obstacle.transform.localPosition = new Vector3(randomX, startYPosition, 0);
            obstacle.AddComponent<GameObjectMoverScript>();

        }
    }
}
