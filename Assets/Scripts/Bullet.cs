using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletInfo info;
    private Rigidbody rb;
    private PhotonView pv;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        info.render = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<PlayerSetting>().takeDamage(info.damage);
            PhotonNetwork.Destroy(gameObject);
        }
    }
    public void startMove(Vector3 direction)
    {
        rb.velocity = direction * info.speed;
    }
}
