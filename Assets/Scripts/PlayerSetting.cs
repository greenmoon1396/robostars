using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerSetting : MonoBehaviourPunCallbacks
{
    private PhotonView pv;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;

    private GameNetworkManager gameManager;
    private const byte GAME_IS_WIN = 0;

    private int protection;

    private void Start()
    {
        protection = 0;
        maxHealth += gameObject.GetComponentInParent<PlayerBuff>().getCurrentHealth();
    }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        gameManager = gameObject.GetComponentInParent<GameNetworkManager>();
    }
    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEventCome;
    }
    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEventCome;
    }

    private void OnNetworkEventCome(EventData eventData)
    {
        if (eventData.Code == GAME_IS_WIN && pv.IsMine)
        {
            gameManager.OnGameWin.Invoke();
        } 
    }

    private void SendWinEvent()
    {
        protection += 3;
        object[] datas = null;
        PhotonNetwork.RaiseEvent(GAME_IS_WIN, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void takeDamage(int value)
    {
        pv.RPC("updateHealth", RpcTarget.All, value);
    }

    [PunRPC]
    public void updateHealth(int value)
    {
        Debug.Log(health);
        value -= protection;
        if (value < 0) return;
        health -= value;
        if(health<=0)
        {
            if(!pv.IsMine)
            {
                return;
            }
            //health = maxHealth;
            //transform.GetComponentInChildren<PlayerController>().Respawn();
            SendWinEvent();
            gameManager.OnGameOver.Invoke();
        }
        healthBar.value = health;
        if(health>maxHealth/2)
        {
            protection += 1;
        }
    }
}
