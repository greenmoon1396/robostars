using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private CameraFlow myCameraFlow;
    private PhotonView pv;
    private Animator animator;
    private CharacterController characterController;
    private PlayerInput inputActions;
    private Vector2 movementInput;
    private Vector3 currentMovement;
    private Quaternion rotatePlayer;
    [SerializeField] private float rotateSpeed;
    private bool isWalk;
    private bool isRun;

    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();
        characterController= GetComponent<CharacterController>();
        animator= GetComponent<Animator>();
        inputActions= new PlayerInput();

        inputActions.CharacterControls.Movement.started += OnMovementActions;
        inputActions.CharacterControls.Movement.performed += OnMovementActions;
        inputActions.CharacterControls.Movement.canceled += OnMovementActions;

        inputActions.CharacterControls.Movement.started += OnCameraMovement;
        inputActions.CharacterControls.Movement.performed += OnCameraMovement;
        inputActions.CharacterControls.Movement.canceled += OnCameraMovement;

        inputActions.CharacterControls.Run.started += OnRun;
        inputActions.CharacterControls.Run.canceled += OnRun;

        if (!pv.IsMine)
        {
            Destroy(myCameraFlow.gameObject);
        }
    }

    private void OnEnable()
    {
        inputActions.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        inputActions.CharacterControls.Disable();
    }

    private void OnMovementActions(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        currentMovement.x = movementInput.x;
        currentMovement.z = movementInput.y;

        isWalk = movementInput.x != 0 || movementInput.y != 0;
    }

    private void PlayerRotate()
    {
        if(isWalk)
        {

            rotatePlayer = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(currentMovement),
                rotateSpeed*Time.deltaTime
            );
            transform.rotation = rotatePlayer;
        }
    }

    private void AnimateControl()
    {
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        characterController.Move(currentMovement * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (!pv.IsMine) return;
        AnimateControl();
        PlayerRotate();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }

    private void OnCameraMovement(InputAction.CallbackContext context) {
        myCameraFlow.setOffset(currentMovement);
    }

    public void Respawn()
    {
        characterController.enabled = false;
        transform.position = Vector3.up;  
        characterController.enabled = true;
    }
}

