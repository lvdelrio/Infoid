using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject player;
    public GameController gameController;
    public Text distanceText;

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
    }

    void Update()
    {
        UpdateDistanceUI();
    }

    private void UpdateDistanceUI()
    {
        if (distanceText != null && gameController != null)
        {
            float totalDistance = gameController.GetTotalDistance();
            distanceText.text = Mathf.FloorToInt(totalDistance) + " m";
        }
    }
}
