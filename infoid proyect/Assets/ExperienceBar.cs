using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text LevelText;

    public void UpdateExperienceBar(int current, int target){
        slider.maxValue = target;
        slider.value = current;
    }

    public void SetLevelText(int level){
        LevelText.text = "Level : " + level.ToString();
    }
}

