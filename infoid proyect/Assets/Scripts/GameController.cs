using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject levelGenerator;
    public GameObject enemySpawner;

    public int score;
    public int currentLevel;
    private float _distance;
    private float _currentDistance;



    void Start()
    {
        score = 0;
        currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        _currentDistance = Vector2.Distance(player.transform.position, new Vector2(0,0));
        if (_currentDistance > _distance)
        {
            _distance = _currentDistance;
        }
    }

    public void IncreaseScore(int value)
    {
        score += value;
    }

    public int GetScore()
    {
        return score;
    }

    public float GetDistance()
    {
        return _distance;
    }



    public void EndLevel()
    {
        currentLevel++;
        player.GetComponent<PlayerController>().ResetPlayerPosition();
        camera.GetComponent<CameraController>().ResetCameraPosition();
        levelGenerator.GetComponent<LevelGeneratorController>().ResetLevelGenerator();
        enemySpawner.GetComponent<SimpleSpawnerController>().DestroyAllEnemies();
    }

    public bool RollLuck(int minNumber, int maxNumber)
    {
        int roll;
        int luck = player.GetComponent<PlayerController>().luck;
        for (int i = 0; i < luck; i++)
        {
            roll = Random.Range(0, maxNumber);
            if (roll > minNumber)
            {
                return true;
            }
        }
        return false;
    }
    
}
