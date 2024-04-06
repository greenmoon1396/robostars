using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using System.IO;


public class Aim : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> allTarget;
    [SerializeField] private GameObject targetCylinder;
    [SerializeField] private float range;

    private PlayerInput inputs;
    private PhotonView pv;
    private CharacterController characterController;
    private GameObject targetObj;
    private bool canSearch = true;
    private int targetCount;

    private PlayerBuff playerBuff;
    private Animator characterAnimator;

    private short attackState=0; // 0 - balls, 1 - attackByPunch1, 2 - attackByPunch2

    void Awake()
    {
        inputs = new PlayerInput();
        pv = GetComponentInParent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        playerBuff = GetComponentInParent<PlayerBuff>();
        characterAnimator= GetComponentInParent<Animator>();
    }

    private void Start()
    {
        if (!pv.IsMine) return;
        targetCylinder.SetActive(false);
        inputs.CharacterControls.ChangeTarget.started += SelectNewTarget;
        inputs.CharacterControls.Fire.started+= OnFire;

    }

    private void OnEnable()
    {
        inputs.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        inputs.CharacterControls.Disable(); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void SetTargetStatus(bool isTarget)
    {
        targetCylinder.SetActive(isTarget);
    }

    public void SelectTarget()
    {
        if(characterController.velocity == Vector3.zero)
        {
            if(canSearch)
            {
                InvokeRepeating("Calculate", 0f, 0.5f);
            }
        } else
        {
            if(targetObj != null)
            {
                targetObj.GetComponent<Aim>().SetTargetStatus(false);
                targetObj = null;
            }
            canSearch= true;
            CancelInvoke();
        }
    }

    private void Calculate()
    {
        canSearch = false;
        allTarget.Clear();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.position, range);
        foreach (RaycastHit hit in hits)
        {
            GameObject targetObj = hit.collider.gameObject;
            if(targetObj.GetComponent<CharacterController>() && 
               !targetObj.GetComponentInParent<PhotonView>().IsMine) { 
                allTarget.Add(targetObj);
            }
        }
        if (allTarget.Count == 0) return;
        SelectNewTarget();
    }

    private void SelectNewTarget()
    {
        if (allTarget.Count == 0) return;
        foreach (GameObject obj in allTarget)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }
        if (targetCount >= allTarget.Count)
        {
            targetCount = 0;
        }
        targetObj = allTarget[targetCount];
        allTarget[targetCount].GetComponent<Aim>().SetTargetStatus(true);
    }

    private void SelectNewTarget(InputAction.CallbackContext context)
    {
        targetCount++;
        if (allTarget.Count == 0) return;
        foreach (GameObject obj in allTarget)
        { 
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }
        if(targetCount >= allTarget.Count)
        {
            targetCount = 0;
        }
        targetObj = allTarget[targetCount];
        allTarget[targetCount].GetComponent<Aim>().SetTargetStatus(true);
    }
    
    
    void FixedUpdate()
    {
        if (!pv.IsMine) return;
        SelectTarget();
    }

    private void OnFire(InputAction.CallbackContext context)
    {

        if(playerBuff.getCurrentHealth()>0 && playerBuff.getCurrentHealth() < 10 )
        {
            attackState = 2;
        } else if (playerBuff.getCurrentHealth() > 10)
        {
            attackState = 1;
        } else
        {
            attackState = 0;
        }
        attack();
    }

    private void attack()
    {
        if (targetObj == null) return;
        Vector3 direction = (targetObj.transform.position - transform.position).normalized;
        string particleSystemName = "FireBall";
        if (attackState!=0)
        {
            particleSystemName = "TransparentFireBall";
        }
        GameObject temp = PhotonNetwork.Instantiate(
            Path.Combine(particleSystemName),
            spawnPosition.position,
            Quaternion.identity);
        temp.GetComponent<Bullet>().startMove(direction);
        Physics.IgnoreCollision(temp.GetComponent<Collider>(), transform.GetComponent<Collider>());
        //animation 
        if (attackState == 1) attackByPunch1();
        else if(attackState == 2) attackByPunch2();   
    }
    private void attackByPunch1()
    {
        characterAnimator.SetTrigger("isPunch1");
    }
    private void attackByPunch2()
    {
        characterAnimator.SetTrigger("isPunch2");
    }
}
