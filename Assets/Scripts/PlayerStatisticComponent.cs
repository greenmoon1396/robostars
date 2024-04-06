using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticComponent : MonoBehaviour
{
    public int countToPoints;
    public int currentExp;
    public int EXPtoLevel;
    public int currentLevel;
    public const int START_EXP_VALUE = 500; // to next level START_EXP_VALUE*currentLevel

    public void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel");
        if (currentLevel == 0) currentLevel = 1;
        currentExp = PlayerPrefs.GetInt("CurrentEXP");
        if (currentExp == 0) currentExp = 1;
        countToPoints = PlayerPrefs.GetInt("CountBuffPoints");
        updateExpToNextLevel();
    }

    public void updateExpToNextLevel()
    {
        EXPtoLevel = START_EXP_VALUE * currentLevel;
    }

    public int getCountToPoints()
    {
        return countToPoints;
    }
    public void takeCountToPoints()
    {
        countToPoints--;
        saveData();
    }
    public void saveData()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("CurrentEXP", currentExp);
        PlayerPrefs.SetInt("CountBuffPoints", countToPoints);
    }
}
