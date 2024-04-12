using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    PlayerController player;
    Text distanceText;
    private void Awake() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        distanceText = GameObject.Find("UiDist").GetComponent<Text>();
    }

    void Start()
    {
        
    }
    void Update()
    {
        int  distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance+" m";
        
    }
}
