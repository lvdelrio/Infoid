using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject levelGenerator;
    public GameObject camera;
    public GameObject enemySpawner;
    public GameObject eventSystem;
    public GameObject blockHandPreFab;

    public int score;
    public int currentLevel;
    private float _distance;
    private float _currentDistance;
    private Vector2 _lastPosition;
    private float _totalDistance;
    private float _currentLevelDistance;
    private float _highestYPosition;



    void Start()
    {
        score = 0;
        currentLevel = 0;
        _lastPosition = player.transform.position;
        _totalDistance = GameStats.TotalDistance;
        _currentLevelDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = player.transform.position;
        float distanceMoved = Vector2.Distance(_lastPosition, currentPosition);
        float currentY = player.transform.position.y;
        if(currentY < _highestYPosition){
            float fallenDistance = _highestYPosition - currentY;
            _totalDistance += fallenDistance;
            _currentLevelDistance += fallenDistance;
            _highestYPosition = currentY;
        }
        else if (currentY > _highestYPosition)
        {
            // Update the highest point if the player goes higher
            _highestYPosition = currentY;
        }

    }


    public void FinishGame()
    {
        GameStats.TotalDistance = _totalDistance;
        //isntanciar al bloque que te persigue y termina el juego
        Instantiate<GameObject>(blockHandPreFab, player.transform.position + new Vector3(0,30,0), Quaternion.identity);
        //me lo hizo el bot pero funciona
        /* #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif */
    }
    public void finish(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void EndLevel()
    {
        // if (currentLevel == 0){
        //     eventSystem.GetComponent<LoadNextScene>().LoadNextLevel();
        // }
        GameStats.TotalDistance = _totalDistance;
        //currentLevel++;
        player.GetComponent<PlayerController>().ResetPlayerPosition();
        camera.GetComponent<CameraController>().ResetCameraPosition();
        levelGenerator.GetComponent<LevelGeneratorController>().ResetLevelGenerator();
        enemySpawner.GetComponent<SimpleSpawnerController>().DestroyAllEnemies();
        _currentLevelDistance = 0;
        _highestYPosition = player.transform.position.y;
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
        public float GetCurrentLevelDistance()
    {
        return _currentLevelDistance;
    }

    public float GetTotalDistance()
    {
        return _totalDistance;
    }
    
}
