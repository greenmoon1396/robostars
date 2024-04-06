using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<PhotonView>();
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex >= 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("GameManager"), 
                new Vector3(Random.value* Random.Range(-1, 1), 0, Random.value * Random.Range(-1, 1)), 
                Quaternion.identity);
        }
    }
}
