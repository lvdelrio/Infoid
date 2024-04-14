using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGeneratorController : MonoBehaviour
{
    private const float PLAYER_DISTANCE = 50F;
    [SerializeField] private Transform _levelSegmentStart;
    [SerializeField] private List<Transform> _levelSegment;
    [SerializeField] private PlayerController _player; 
    [SerializeField] private List<GameObject> _obstaclePrefabs;
    [SerializeField] private GameObject _edgeColliderPrefab;

    private Vector3 _lastPosition;
    private List<Transform> _activeSegments = new List<Transform>();
    public Transform initialSegmentPosition;
    private Transform _instantiatedSegmentStart;

    public int levelSegmentCount = 5;
    private int _currentLevelSegmentCount = 0;

    private void Awake() {
        _instantiatedSegmentStart = Instantiate(initialSegmentPosition, new Vector3(0f,0f,0f), Quaternion.identity);
        _lastPosition = _instantiatedSegmentStart.Find("end_segment").position;
        for (int i=0; i<_levelSegment.Count; i++){
            SpawnSegment();
        }
        
    }
    
    private void Update() {
        if (_instantiatedSegmentStart != null && Vector3.Distance(_player.GetPosition(), _instantiatedSegmentStart.position) > PLAYER_DISTANCE) {
        Destroy(_instantiatedSegmentStart.gameObject);
        _instantiatedSegmentStart = null;
        Debug.Log("Destroying starting stage segment");
        }
        if(Vector3.Distance(_player.GetPosition(), _lastPosition) < PLAYER_DISTANCE) {
            SpawnSegment();
        }
        DestroyOldSegments();

    }

    private void SpawnSegment(){
        Transform chosenSegmentPart = _levelSegment[Random.Range(0,_levelSegment.Count)];
        Transform lastlevelpartTransform = CreateSegment(chosenSegmentPart, _lastPosition);
        SpawnObstacle(lastlevelpartTransform.Find("Wall_left"),lastlevelpartTransform.Find("Wall_right"));
        _lastPosition = lastlevelpartTransform.Find("end_segment").position;
        _activeSegments.Add(lastlevelpartTransform);
        if (_currentLevelSegmentCount >= levelSegmentCount) {
            Instantiate(_edgeColliderPrefab, lastlevelpartTransform.position, Quaternion.identity);
            _currentLevelSegmentCount = 0;
        }


        _currentLevelSegmentCount++;

    }

    private Transform CreateSegment(Transform segment_part, Vector3 spawn_position){
        Transform levelpartTransform = Instantiate(segment_part, spawn_position, Quaternion.identity);
        return levelpartTransform;
    } 

    private void SpawnObstacle(Transform wallLeft, Transform wallRight) {
        if (_obstaclePrefabs == null || _obstaclePrefabs.Count == 0) {
            Debug.LogError("Obstacle prefabs list is empty or not assigned!");
            return;
        }

        // Calculate positions based on the wall positions
        float wallLeftPosition = wallLeft.position.x + (wallLeft.GetComponent<Collider2D>().bounds.size.x / 2);
        float wallRightPosition = wallRight.position.x - (wallRight.GetComponent<Collider2D>().bounds.size.x / 2);
        float spaceBetweenWalls = wallRightPosition - wallLeftPosition;

        // Randomly pick an obstacle prefab
        GameObject obstaclePrefab = _obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Count)];
        Collider2D obstacleCollider = obstaclePrefab.GetComponent<Collider2D>();
        if (obstacleCollider == null) {
            Debug.LogError("Obstacle prefab does not have a Collider2D component!");
            return;
        }

        float obstacleWidth = Mathf.Clamp(7f, 2f, spaceBetweenWalls);
        Debug.Log("soy tamaño supuesto de los obstacle"+obstacleWidth+" "+spaceBetweenWalls+""+obstacleCollider.bounds.size.x);
        float minPosX = wallLeftPosition + obstacleWidth / 2;
        float maxPosX = wallRightPosition - obstacleWidth / 2;
        float obstacleXPosition = Random.Range(minPosX, maxPosX);
        Vector3 obstaclePosition = new Vector3(obstacleXPosition, wallLeft.position.y, 0); 
        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
        obstacle.transform.localScale = new Vector3(obstacleWidth, obstacle.transform.localScale.y, obstacle.transform.localScale.z);
    }
    
    private void DestroyOldSegments() {
        for (int i = _activeSegments.Count - 1; i >= 0; i--) {
            if (Vector3.Distance(_player.GetPosition(), _activeSegments[i].position) > PLAYER_DISTANCE) {
                Destroy(_activeSegments[i].gameObject);
                _activeSegments.RemoveAt(i);  
            }
        }
    }



}
