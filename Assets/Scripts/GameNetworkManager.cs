using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;
    [SerializeField] private GameObject allUI;
    private PhotonView pv;

    private void Awake()
    {
        pv = gameObject.GetPhotonView();
    }

    private void Start()
    {
        if(!pv.IsMine)
        {
            allUI.SetActive(false);
            return;
        }
    }

    public void outOfButtle()
    { 
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
        PhotonNetwork.Destroy(pv);
    }
}
