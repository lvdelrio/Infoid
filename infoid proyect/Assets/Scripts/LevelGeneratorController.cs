using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGeneratorController : MonoBehaviour
{
    private const float PLAYER_DISTANCE = 60f;
    [SerializeField] private GameController gameController;
    [SerializeField] private Transform _levelSegmentStart;
    [SerializeField] private List<Transform> _levelSegment;
    [SerializeField] private PlayerController _player; 
    [SerializeField] private List<GameObject> _obstaclePrefabs;
    [SerializeField] private List<GameObject> _powerUpPrefabs;
    [SerializeField] private GameObject _edgeColliderPrefab;

    public Material[] materialBackground;
    public Material[] materialPrefab;
    private Camera _mainCamera;
    private int currentLevel;

    private Vector3 _lastSegmentPosition;

    private List<Transform> _activeSegments = new List<Transform>();
    private List<GameObject> _activeObstacles = new List<GameObject>();
    private List<GameObject> _activePowerUps = new List<GameObject>();

    public Transform initialSegmentPosition;
    private Transform _instantiatedSegmentStart;

    private GameObject _edgeCollider;

    public int levelSegmentCount = 5;
    private int _currentLevelSegmentCount = 0;

    private void Awake() {
        InitiateSegmentCreation();
        _mainCamera = Camera.main;
        
    }
    
    private void Update() {
        if (_instantiatedSegmentStart != null && Vector3.Distance(_player.GetPosition(), _instantiatedSegmentStart.position) > PLAYER_DISTANCE) {
        Destroy(_instantiatedSegmentStart.gameObject);
        _instantiatedSegmentStart = null;
        Debug.Log("Destroying starting stage segment");
        }
        if(Vector3.Distance(_player.GetPosition(), _lastSegmentPosition) < PLAYER_DISTANCE) {
            SpawnSegment();
        }
        

    }

    private void SpawnSegment(){
        Transform chosenSegmentPart = _levelSegment[Random.Range(0,_levelSegment.Count)];
        Transform newestSegment = CreateSegment(chosenSegmentPart, _lastSegmentPosition);
        //SpawnPowerUp(newestSegment.Find("Wall_left"),newestSegment.Find("Wall_right"));
        if(Random.Range(0,2) == 1){
            SpawnObstacle(newestSegment.Find("Wall_left"),newestSegment.Find("Wall_right"));
        }
        if(gameController.RollLuck(8,10)){
            SpawnPowerUp(newestSegment.Find("Wall_left"),newestSegment.Find("Wall_right"));
            Debug.Log("Power up spawned");
        }
        _lastSegmentPosition = newestSegment.Find("end_segment").position;
        _activeSegments.Add(newestSegment);

        if (_currentLevelSegmentCount >= levelSegmentCount) {
            _edgeCollider = Instantiate(_edgeColliderPrefab, newestSegment.position, Quaternion.identity);
            _currentLevelSegmentCount = 0;
        }
        _currentLevelSegmentCount++;
        DestroyOldSegments();
    }


    private Transform CreateSegment(Transform segment_part, Vector3 spawn_position){
        Debug.Log("Creating segment at position: "+spawn_position+"   "+currentLevel);
        Transform newSegment = Instantiate(segment_part, spawn_position, Quaternion.identity);
        Renderer[] renderers = newSegment.GetComponentsInChildren<SpriteRenderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = materialPrefab[currentLevel];
        }
        return newSegment;
    } 

    private void SpawnObject(Transform wallLeft, Transform wallRight, List<GameObject> objectList, List<GameObject> activeObjects) {
        if (objectList == null || objectList.Count == 0) {
            Debug.LogError("Obstacle prefabs list is empty or not assigned!");
            return;
        }

        // Calculate positions based on the wall positions
        float wallLeftPosition = wallLeft.position.x + (wallLeft.GetComponent<Collider2D>().bounds.size.x / 2);
        float wallRightPosition = wallRight.position.x - (wallRight.GetComponent<Collider2D>().bounds.size.x / 2);
        float spaceBetweenWalls = wallRightPosition - wallLeftPosition;

        // Randomly pick an obstacle prefab
        GameObject objectPrefab = objectList[Random.Range(0, objectList.Count)];
        Collider2D objectCollider = objectPrefab.GetComponent<Collider2D>();
        if (objectCollider == null) {
            Debug.LogError("Obstacle prefab does not have a Collider2D component!");
            return;
        }

        float objectWidth = Mathf.Clamp(7f, 2f, spaceBetweenWalls);
        Debug.Log("soy tamaño supuesto de los obstacle"+objectWidth+" "+spaceBetweenWalls+""+objectCollider.bounds.size.x);
        float minPosX = wallLeftPosition + objectWidth / 2;
        float maxPosX = wallRightPosition - objectWidth / 2;
        float objectXPosition = Random.Range(minPosX, maxPosX);
        Vector3 obstaclePosition = new Vector3(objectXPosition, wallLeft.position.y, 0); 
        GameObject createdObject = Instantiate(objectPrefab, obstaclePosition, Quaternion.identity);
        activeObjects.Add(createdObject);
        createdObject.transform.localScale = new Vector3(objectWidth, createdObject.transform.localScale.y, createdObject.transform.localScale.z);
        DestroyOldObstacles();
    }

    private void SpawnObstacle(Transform wallLeft, Transform wallRight) {
        SpawnObject(wallLeft, wallRight, _obstaclePrefabs, _activeObstacles);
    }

    private void SpawnPowerUp(Transform wallLeft, Transform wallRight) {
        SpawnObject(wallLeft, wallRight, _powerUpPrefabs, _activePowerUps);
    }

    private void InitiateSegmentCreation(){
        _instantiatedSegmentStart = Instantiate(initialSegmentPosition, new Vector3(0f,0f,0f), Quaternion.identity);
        _lastSegmentPosition = _instantiatedSegmentStart.Find("end_segment").position;
        for (int i=0; i<_levelSegment.Count; i++){
            SpawnSegment();
        }
    }
    
    private void DestroyOldSegments() {
        for (int i = _activeSegments.Count - 1; i >= 0; i--) {
            if (Vector3.Distance(_player.GetPosition(), _activeSegments[i].position) > PLAYER_DISTANCE*2) {
                Destroy(_activeSegments[i].gameObject);
                Debug.Log("Destroying segment at position: "+_activeSegments[i].position);
                _activeSegments.RemoveAt(i);  
            }
        }
    }

    private void DestroyOldObstacles() {
        for (int i = _activeObstacles.Count - 1; i >= 0; i--) {
            if (Vector3.Distance(_player.GetPosition(), _activeObstacles[i].transform.position) > PLAYER_DISTANCE*2) {
                Destroy(_activeObstacles[i]);
                Debug.Log("Destroying obstacle at position: "+_activeSegments[i].position);
                _activeObstacles.RemoveAt(i);
            }
        }
    }

    private void DestroyOldPowerUps() {
        for (int i = _activePowerUps.Count - 1; i >= 0; i--) {
            if (Vector3.Distance(_player.GetPosition(), _activePowerUps[i].transform.position) > PLAYER_DISTANCE*2) {
                Destroy(_activePowerUps[i]);
                Debug.Log("Destroying power up at position: "+_activeSegments[i].position);
                _activePowerUps.RemoveAt(i);
            }
        }
    }	

    private void DestroyAllSegments() {
        foreach (Transform segment in _activeSegments) {
            Destroy(segment.gameObject);
        }
        _activeSegments.Clear();
    }

    private void DestroyAllObstacles() {
        foreach (GameObject obstacle in _activeObstacles) {
            Destroy(obstacle);
        }
        _activeObstacles.Clear();
    }

    private void DestroyAllPowerUps() {
        foreach (GameObject powerUp in _activePowerUps) {
            Destroy(powerUp);
        }
        _activePowerUps.Clear();
    }

    public void ResetLevelGenerator() {
        currentLevel++;
        DestroyAllSegments();
        DestroyAllObstacles();
        DestroyAllPowerUps();

        InitiateSegmentCreation();
        _currentLevelSegmentCount = 0;

        //cambiar de nivel al volver al principio, cambiando de color el background de la camara
        _mainCamera.backgroundColor = materialBackground[currentLevel].color;
            // Cambiar el material de los prefabs
        // foreach (GameObject prefab in spawnedPrefabs) {
        //     prefab.GetComponent<Renderer>().material = materialPrefab[currentLevel];
        // }

    }
}
