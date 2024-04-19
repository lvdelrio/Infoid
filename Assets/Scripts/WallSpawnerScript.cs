// Encargado de crear nuevas murrallas

using UnityEngine;
using Infoid.Enums;

public class GameScript : MonoBehaviour
{
    public Camera mainCamera;
    private float screenHeight;
    private float screenWidth;
    private GameObject wallPrefab;
    private GameObject spikePrefab;
    private GameObject wall;

    void Start()
    {
        screenHeight = 2f * mainCamera.orthographicSize;
        screenWidth = screenHeight * mainCamera.aspect;
        wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");
        spikePrefab = Resources.Load<GameObject>("Prefabs/Spike");

        SpawnWall(Side.Left, Position.OnScreen);
        SpawnWall(Side.Right, Position.OnScreen);
        SpawnWall(Side.Left, Position.AfterScreen);
        SpawnWall(Side.Right, Position.AfterScreen);
    }


    void Update()
    {
        if (!Config.DEBUG && ShouldSpawnWalls())
        {
            SpawnWall(Side.Left, Position.AfterScreen);
            SpawnWall(Side.Right, Position.AfterScreen);
        }
    }


    private void SpawnWall(Side side, Position position)
    {
        wall = Instantiate(wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // Seteamos el ancho de la murralla como el ancho de la pantalla / 10
        // Seteamos el alto de la murralla como el alto de la pantalla 
        wall.transform.localScale = new Vector3(screenWidth / Config.PART_OF_SCREEN_THAT_HAS_WALL, screenHeight, 1);

        // Agreamos un tag para saber de que lado esta la muralla
        wall.tag = side == Side.Left ? "Left Wall" : "Right Wall";

        // Pondemos su posición dependiendo de que lado es
        float xPosition = side == Side.Left ? mainCamera.transform.position.x - screenWidth / 2f + wall.transform.localScale.x / 2f : mainCamera.transform.position.x + screenWidth / 2f - wall.transform.localScale.x / 2f;
        wall.transform.position = new Vector3(xPosition, mainCamera.transform.position.y, 0);

        if (position == Position.AfterScreen)
        {
            float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
            float wallHeight = wall.transform.localScale.y;
            float startYPosition = cameraBottom - wallHeight / 2.0f + 0.1f;

            wall.transform.position = new Vector3(xPosition, startYPosition, 0);
        }

        // Agregamos random un spike
        if (Random.Range(0, 2) == 0)
        {
            GameObject newSpike = Instantiate(spikePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newSpike.transform.SetParent(wall.transform);
            newSpike.transform.localPosition = Vector3.zero;

            if (side == Side.Right)
            {
                // Lo ponemos en una posición random de la muralla
                float randomY = Random.Range(-0.5f, 0.5f);
                newSpike.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                newSpike.transform.localPosition = new Vector3(newSpike.transform.localPosition.x, randomY, 0f);
            }
        }

        if (!Config.DEBUG)
        {
            // Agregamos GameObjectMoverScript a murralla
            wall.AddComponent<GameObjectMoverScript>();
        }
    }


    private bool ShouldSpawnWalls()
    {
        // Wall guarda la ultima muralla que se creo (no importa el lado porque se mueven con la misma velocidad)
        if (!wall) return false;

        float wallHeight = wall.transform.localScale.y;
        float bottomEdgePosition = wall.transform.position.y - wallHeight / 2.0f;
        Vector3 bottomEdgeScreenPoint = mainCamera.WorldToViewportPoint(new Vector3(wall.transform.position.x, bottomEdgePosition, wall.transform.position.z));

        if (bottomEdgeScreenPoint.y > 0)
        {
            wall = null;
            return true;
        }

        return false;
    }
}
