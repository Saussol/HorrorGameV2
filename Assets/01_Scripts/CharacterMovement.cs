using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Unity.Netcode;
using System;

public enum PlayerState
{
    NORMAL,
    RAT
}

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Title("Camera Control")]
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform rightHand;

    [Title("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float stunSpeed;
    [SerializeField] private float jumpHeight;

    [Title("Crouch")]
    [SerializeField] private float standingHeight;
    [SerializeField] private float crouchingHeight;

    private CharacterController characterController;
    private Camera playerCamera;

    [SerializeField]
    private Animator animator;

    private float yaw;
    private float pitch;

    private Vector3 velocity = Vector3.zero;
    private float gravity = 9.81f;
    private bool isCrouching;
    private bool isStun;
    private float crouchTransitionSpeed = .1f;

    private bool canMove = true;
    private bool isStopped;

    private PlayerState _playerState = PlayerState.NORMAL;
    public PlayerState CheckPlayerState()
	{
        return _playerState;
	}

    [HideInInspector] public UnityEvent _onRatTransformation;

	private void Awake()
	{
        if (_onRatTransformation == null) _onRatTransformation = new UnityEvent();
	}


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (isStopped) return;

		switch (_playerState)
		{
			case PlayerState.NORMAL:
                HandleMouseLook();

                if (!canMove)
                {
                    velocity.y -= gravity * Time.deltaTime;
                    characterController.Move(velocity * Time.deltaTime);

                    return;
                }


                HandleMovement();
                HandleCrouch();
                break;
			case PlayerState.RAT:
                HandleMouseLook();

                if (!canMove)
                {
                    velocity.y -= gravity * Time.deltaTime;
                    characterController.Move(velocity * Time.deltaTime);

                    return;
                }

                HandleMovement();
                break;
		}
    }

    public PlayerState GetPlayerState()
	{
        return _playerState;
	}

    private void HandleMovement()
    {
        float currentSpeed = moveSpeed;

        switch (_playerState)
		{
			case PlayerState.NORMAL:
                currentSpeed = isStun ? stunSpeed : isCrouching ? crouchSpeed : Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
                //Anim

                break;
			case PlayerState.RAT:
                currentSpeed = sprintSpeed;

                break;
		}

        float horizontal = Input.GetAxis("Horizontal") * currentSpeed;
        float vertical = Input.GetAxis("Vertical") * currentSpeed;

        if (horizontal == 0 && vertical == 0)
        {
            animator.Play("Idle");
        }
        else if (currentSpeed == moveSpeed)
        {
            animator.Play("Walk");
        }
        else if (currentSpeed == sprintSpeed)
        {
            animator.Play("Run");
        }
        else if (currentSpeed == stunSpeed)
        {
            animator.Play("Stun");
        }

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);
        moveDirection = transform.rotation * moveDirection;

        HandleJump();

        velocity.x = moveDirection.x;
        velocity.z = moveDirection.z;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        yaw += Input.GetAxisRaw("Mouse X") * lookSensitivity;
        pitch -= Input.GetAxisRaw("Mouse Y") * lookSensitivity;

        pitch = ClampAngle(pitch, minPitch, maxPitch);

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
        rightHand.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            velocity.y = -.5f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpHeight;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isStun)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private IEnumerator CrouchStand()
    {
        float targetHeight = isCrouching ? standingHeight : crouchingHeight;
        float initialHeight = characterController.height;

        float timeElapsed = 0f;

        while (timeElapsed < crouchTransitionSpeed)
        {
            characterController.height = Mathf.Lerp(initialHeight, targetHeight, timeElapsed / crouchTransitionSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        isCrouching = !isCrouching;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    public void StunPlayer()
	{
        if (_playerState == PlayerState.RAT) return;

        StartCoroutine(Stun());
	}

    private IEnumerator Stun()
	{
        isStun = true;
        float baseFOV = playerCamera.fieldOfView;

        Camera childCam = playerCamera.transform.GetChild(0).GetComponent<Camera>();
        Debug.Log(childCam.gameObject.name);

        while(playerCamera.fieldOfView > 30)
		{
            playerCamera.fieldOfView -= .3f;
            childCam.fieldOfView = playerCamera.fieldOfView;
            yield return null;
		}
        playerCamera.fieldOfView = 30;
        childCam.fieldOfView = playerCamera.fieldOfView;

        yield return new WaitForSeconds(5f);

        isStun = false;

        while (playerCamera.fieldOfView < baseFOV)
        {
            playerCamera.fieldOfView += .3f;
            childCam.fieldOfView = playerCamera.fieldOfView;
            yield return null;
        }
        playerCamera.fieldOfView = baseFOV;
        childCam.fieldOfView = playerCamera.fieldOfView;
    }

    public void StopPlayer()
	{
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isStopped = true;
    }

    public void StartScreamer()
	{
        isStopped = true;
	}

    [ContextMenu("Rat Transformation")]
    public void RatTransformation(Vector3 respawnPosition, bool endGame)
	{
		if (QuestSpawner.Instance.firePickedUp)
		{
            QuestSpawner.Instance.firePickedUp = false;
            QuestSpawner.Instance._restoreFire.Invoke();
        }

        isStopped = false;

        _playerState = PlayerState.RAT;
        _onRatTransformation.Invoke();
        characterController.height = crouchingHeight;
        transform.localScale = new Vector3(1, .5f, 1);

        if(!endGame)
            StartCoroutine(TeleportPlayer(respawnPosition));
        else
            transform.position = respawnPosition;
    }

    public IEnumerator TeleportPlayer(Vector3 teleportPosition)
	{
        isStopped = true;
        yield return new WaitForSeconds(.1f);
        transform.position = teleportPosition;
        yield return new WaitForSeconds(.1f);
        isStopped = false;
    }
}
