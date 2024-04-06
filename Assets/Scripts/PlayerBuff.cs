using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBuff : MonoBehaviourPunCallbacks
{
    public int health; // 1000
    public int currentHealth; //768
    private void Awake()
    {
        loadBuffs();
    }
    public int getCurrentHealth()
    {
        return currentHealth;
    }
    public void loadBuffs()
    {
        currentHealth = PlayerPrefs.GetInt("CurrentHealth");
    }

    public void saveBuffs()
    {
        PlayerPrefs.SetInt("CurrentHealth", currentHealth);
    }

    public void upgrateBuffs()
    {
        MenuPlayerStatisticComponent playerStatisticComponent = GetComponent<MenuPlayerStatisticComponent>();
        if(playerStatisticComponent.getCountToPoints()>0)
        {
            playerStatisticComponent.takeCountBuffPoints();
            currentHealth = currentHealth + health;
            saveBuffs();
        }
    }

}
