using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_generatorController : MonoBehaviour
{
    private const float PLAYER_DISTANCE = 50F;
    [SerializeField] private Transform level_segment_start;
    [SerializeField] private List<Transform> level_segment;
    [SerializeField] private PlayerController player; 
    [SerializeField] private List<GameObject> obstaclePrefabs;

    private Vector3 lastposition;
    private List<Transform> activeSegments = new List<Transform>();
    public Transform level_segment1_start;
    private Transform instantiatedSegmentStart;

    private void Awake() {
        instantiatedSegmentStart = Instantiate(level_segment1_start, new Vector3(0f,0f,0f), Quaternion.identity);
        lastposition = instantiatedSegmentStart.Find("end_segment").position;
        for (int i=0; i<level_segment.Count; i++){
            spawn_segment();
        }
        
    }
    
    private void Update() {
        if (instantiatedSegmentStart != null && Vector3.Distance(player.GetPosition(), instantiatedSegmentStart.position) > PLAYER_DISTANCE) {
        Destroy(instantiatedSegmentStart.gameObject);
        instantiatedSegmentStart = null;
        Debug.Log("Destroying starting stage segment");
        }
        if(Vector3.Distance(player.GetPosition(), lastposition) < PLAYER_DISTANCE) {
            spawn_segment();
        }
        DestroyOldSegments();

    }

    private void spawn_segment(){
        Transform chosenSegmentPart = level_segment[Random.Range(0,level_segment.Count)];
        Transform lastlevelpartTransform = spawn_segments(chosenSegmentPart, lastposition);
        SpawnObstacle(lastlevelpartTransform.Find("Wall_left"),lastlevelpartTransform.Find("Wall_right"));
        lastposition = lastlevelpartTransform.Find("end_segment").position;
        activeSegments.Add(lastlevelpartTransform);
    }

     private Transform spawn_segments(Transform segment_part, Vector3 spawn_position){
        Transform levelpartTransform = Instantiate(segment_part, spawn_position, Quaternion.identity);
        return levelpartTransform;
    } 
private void SpawnObstacle(Transform wallLeft, Transform wallRight) {
    if (obstaclePrefabs == null || obstaclePrefabs.Count == 0) {
        Debug.LogError("Obstacle prefabs list is empty or not assigned!");
        return;
    }

    // Calculate positions based on the wall positions
    float wallLeftPosition = wallLeft.position.x + (wallLeft.GetComponent<Collider2D>().bounds.size.x / 2);
    float wallRightPosition = wallRight.position.x - (wallRight.GetComponent<Collider2D>().bounds.size.x / 2);
    float spaceBetweenWalls = wallRightPosition - wallLeftPosition;

    // Randomly pick an obstacle prefab
    GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
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
        for (int i = activeSegments.Count - 1; i >= 0; i--) {
            if (Vector3.Distance(player.GetPosition(), activeSegments[i].position) > PLAYER_DISTANCE) {
                Destroy(activeSegments[i].gameObject);
                activeSegments.RemoveAt(i);  
            }
        }
    }



}
