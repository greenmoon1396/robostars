using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbiNetworkManager : MonoBehaviourPunCallbacks
{
    public static LobbiNetworkManager Instance;
    [SerializeField] private TMP_Text waitBattleText;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        WindowsManager.Layout.OpenLayout("Loading");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }

    public void toButtleButton()
    {
        WindowsManager.Layout.OpenLayout("AutomaticBattle");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if(returnCode == (short)ErrorCode.NoRandomMatchFound)
        {
            waitBattleText.text = "No matches found. We are creating a new room.";
            CreateNewRoom();
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.GameIdAlreadyExists)
        {
            CreateNewRoom();
        }
    }

    private string RoomNameGenerator()
    {
        short codeLengths = 12;
        string roomCode = null;
        for(short i=0; i<codeLengths; i++)
        {
            char rand =(char)Random.Range(65, 91);
            roomCode += rand;
        }
        return roomCode;
    }

    private void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RoomNameGenerator(), roomOptions);
    }

    public override void OnCreatedRoom()
    {
        waitBattleText.text = "Waiting for the second player";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient) return;
        waitBattleText.text = "The battle begins! Get ready";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(!PhotonNetwork.IsMasterClient) return;

        Room currentRoom = PhotonNetwork.CurrentRoom;
        currentRoom.IsOpen = false;
        waitBattleText.text = "The battle begins! Get ready";
        Invoke("LoadingGameMap", 3f);
    }
    private void LoadingGameMap()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void StopFindButtle()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }
}
