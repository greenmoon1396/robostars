using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using ExitGames.Client.Photon;
using Photon.Pun.Demo.PunBasics;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] private float radiusSearch;
    private Transform targetTransform;

    [SerializeField] private float timeAttack;
    [SerializeField] private int damage;
    private bool isAttack;

    private NavMeshAgent agent;
    private Animator enemyAnimation;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
    }

    [PunRPC]
    private void searchTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusSearch);
        foreach(Collider hitCollider in hitColliders)
        {
            if(hitCollider.GetComponent<CharacterController>())
            {
                targetTransform = hitCollider.GetComponent<CharacterController>().transform;
            }
        }
    }

    private void Update()
    {
        if(targetTransform == null) {
            photonView.RPC("searchTarget", RpcTarget.All);
        }  else if(Time.frameCount%50 == 0)
        {
            Action();
        }
    }

    private IEnumerator startAttack()
    {
        if (targetTransform == null) yield return null;
        isAttack = true;
        yield return new WaitForSeconds(timeAttack);
       
        targetTransform.gameObject.GetComponentInParent<PlayerSetting>().takeDamage(damage);
        isAttack = false;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimation = GetComponentInChildren<Animator>();
    }

    private void Action()
    {
        if(Vector3.Distance(targetTransform.position, transform.position) <= agent.stoppingDistance && 
            !isAttack)
        {
            enemyAnimation.SetBool("isAttack", true);
            StartCoroutine(startAttack());
        } else
        {

            enemyAnimation.SetBool("isRun", true);
            agent.SetDestination(targetTransform.position);
        }
    }

    private const byte GAME_END = 0;

    private void OnNetworkEventCome(EventData eventData)
    {
        if (eventData.Code == GAME_END)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEventCome;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEventCome;
    }
}
