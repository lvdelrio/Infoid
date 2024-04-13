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
        lastposition = lastlevelpartTransform.Find("end_segment").position;
        activeSegments.Add(lastlevelpartTransform);
    }

     private Transform spawn_segments(Transform segment_part, Vector3 spawn_position){
        Transform levelpartTransform = Instantiate(segment_part, spawn_position, Quaternion.identity);
        return levelpartTransform;
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
