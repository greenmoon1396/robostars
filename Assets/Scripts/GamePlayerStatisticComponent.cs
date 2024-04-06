using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayerStatisticComponent : PlayerStatisticComponent
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentEXPText;
    [SerializeField] private Image fillEXPImage;
    private const int LOOSE_COEFFICIENT = 1;
    private const int WIN_COEFFICIENT = 3;
    private const int STANDART_EXP_VALUE = 30;

    void updateText()
    {
        levelText.text = "Level: " + currentLevel.ToString();
        currentEXPText.text = currentExp.ToString() + "/" + EXPtoLevel.ToString();
        fillEXPImage.fillAmount = (float)currentExp / (float)EXPtoLevel;
    }

    public void ShowWinInfo()
    {
        int value = currentLevel*STANDART_EXP_VALUE*WIN_COEFFICIENT;
        if(currentExp+value > EXPtoLevel)
        {
            currentExp = (currentExp + value) - EXPtoLevel;
            currentLevel++;
            countToPoints++;
            updateExpToNextLevel();
        } else
        {
            currentExp = currentExp + value;
        }
        titleText.text = "You win!";
        updateText();
        saveData();
    }
    public void ShowLoseInfo()
    {
        int value = currentLevel * STANDART_EXP_VALUE * LOOSE_COEFFICIENT;
        if (currentExp + value > EXPtoLevel)
        {
            currentExp = (currentExp + value) - EXPtoLevel;
            currentLevel++;
            countToPoints++;
            updateExpToNextLevel();
        }
        else
        {
            currentExp = currentExp + value;
        }
        titleText.text = "You loose!";
        updateText();
        saveData();
    }
}
