using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_scene1 : MonoBehaviour
{
    private void OnEnable() {
        SceneManager.LoadScene("Scene_1", LoadSceneMode.Single);
    }
}
