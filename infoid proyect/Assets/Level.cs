using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int level = 1; 
    int experiences = 0;
    [SerializeField] ExperienceBar experienceBar;

    int TO_LEVEL_UP
    {
        get{
            return level * 1000;
        }
    }

    private void Start(){
        experienceBar.UpdateExperienceBar(experiences, TO_LEVEL_UP);
        experienceBar.SetLevelText(level);
    }

    public void AddExperience(int amount){
        experiences += amount;
        checkLevelUp();
        experienceBar.UpdateExperienceBar(experiences, TO_LEVEL_UP);
    }
    void checkLevelUp(){
        if(experiences >= TO_LEVEL_UP){
            experiences -= TO_LEVEL_UP;
            level += 1;
            experienceBar.SetLevelText(level);
        }
    }
}
