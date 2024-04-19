// Encargado de crear nuevos enemigos

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Camera mainCamera;
    private float screenHeight;
    private float screenWidth;
    private GameObject batPrefab;
    private GameObject bat;
    public float spawnDelay = 1.0f;
    private float nextSpawnTime = 0.0f;

    void Start()
    {
        screenHeight = 2f * mainCamera.orthographicSize;
        screenWidth = screenHeight * mainCamera.aspect;
        batPrefab = Resources.Load<GameObject>("Prefabs/Bat");
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        // Hacemos esto porque Update se llama en cada frame (osea casi 60 veces por segundos, con esto hacemos que se llame cada 1 segundo)
        if (Time.time >= nextSpawnTime)
        {
            if (!Config.DEBUG && bat == null && Random.Range(0, 5) == 1)
            {
                bat = Instantiate(batPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
                float obstacleHeight = bat.transform.localScale.y;
                float startYPosition = cameraBottom - obstacleHeight / 2.0f + 0.1f;
                float randomX = Random.Range(screenWidth / 2 * -1 + bat.transform.localScale.x, screenWidth / 2 - bat.transform.localScale.x);

                bat.transform.localPosition = new Vector3(randomX, startYPosition, 0);
                bat.AddComponent<BatScript>();

            }

            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
