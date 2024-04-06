using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerStatisticComponent : PlayerStatisticComponent
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentEXPText;
    [SerializeField] private TMP_Text countToPointsText;
    [SerializeField] private Image fillEXPImage;

    void Start()
    {
        levelText.text = "Level: " + currentLevel.ToString();
        countToPointsText.text = "Upgrate(" + countToPoints.ToString()+")";
        currentEXPText.text = currentExp.ToString()+ "/" + EXPtoLevel.ToString();
        fillEXPImage.fillAmount = (float)currentExp / (float)EXPtoLevel;
    }
    public void takeCountBuffPoints()
    {
        takeCountToPoints();
        countToPointsText.text = "Upgrate(" + countToPoints.ToString() + ")";
    }
}
