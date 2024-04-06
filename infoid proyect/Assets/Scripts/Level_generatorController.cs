using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_generatorController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Transform level_segment_start;
    [SerializeField]private Transform level_segment1;

    private void Awake() {
        Transform lastlevelpartTransform;
        lastlevelpartTransform = spawn_segment(level_segment_start.Find("end_segment").position);
        lastlevelpartTransform = spawn_segment(lastlevelpartTransform.Find("end_segment").position);
        lastlevelpartTransform = spawn_segment(lastlevelpartTransform.Find("end_segment").position);
        
        
    }

    private Transform spawn_segment(Vector3 spawn_position){
        Transform levelpartTransform = Instantiate(level_segment1, spawn_position, Quaternion.identity);
        return levelpartTransform;
    }
}
