using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EnemySpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> enemysPositions;
    [SerializeField] float timeToSpawnEnemy;

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(timeToSpawnEnemy);

        while(enemysPositions.Count > 0)
        {
            int index = Random.Range(0, enemysPositions.Count);
            GameObject enemyObject = PhotonNetwork.Instantiate(
                Path.Combine("Enemy"), 
                enemysPositions[index].position, 
                Quaternion.identity);

            enemysPositions.Remove(enemysPositions[index]);
        }
    }
    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(spawnEnemy());
        }
    }
}
