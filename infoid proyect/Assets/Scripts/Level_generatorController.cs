using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_generatorController : MonoBehaviour
{
    private const float PLAYER_DISTANCE = 50F;
    [SerializeField] private Transform level_segment_start;
    [SerializeField] private List<Transform> level_segment;
    [SerializeField] private PlayerController player; 

    private Vector3 lastposition;
    private void Awake() {
        lastposition =level_segment_start.Find("end_segment").position;
        for (int i=0; i<level_segment.Count;i++){
            spawn_segment();
        }
        
    }
    
    private void Update() {
        if(Vector3.Distance(player.GetPosition(), lastposition) < PLAYER_DISTANCE) {
            spawn_segment();
        }
    }

    private void spawn_segment(){
        Transform chosenSegmentPart = level_segment[Random.Range(0,level_segment.Count)];
        Transform lastlevelpartTransform = spawn_segments(chosenSegmentPart, lastposition);
        lastposition = lastlevelpartTransform.Find("end_segment").position;
    }

     private Transform spawn_segments(Transform segment_part, Vector3 spawn_position){
        Transform levelpartTransform = Instantiate(segment_part, spawn_position, Quaternion.identity);
        return levelpartTransform;
    } 
}
