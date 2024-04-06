using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerBuff : PlayerBuff
{
    private void Awake()
    {
        loadBuffs();
        photonView.RPC("BuffPlayer", RpcTarget.All, PlayerPrefs.GetInt("CurrentHealth"));
    }
    [PunRPC]
    public void BuffPlayer(int currentHlth)
    {
        if (!photonView.IsMine)
        {
            currentHealth = currentHlth;
        }
    }
}
