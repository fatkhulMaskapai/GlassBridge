using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	private InputManager inputManager;
	private CameraManager cameraManager;
	private PlayerLocomotion playerLocomotion;
	private Animator animator;
	private float maxMovementSpeed;
	public bool isInteracting;
	[SerializeField] Transform resetPos;
	private void Awake()
	{
		inputManager = GetComponent<InputManager>();
		cameraManager = FindObjectOfType<CameraManager>();
		playerLocomotion = GetComponent<PlayerLocomotion>();
		animator = GetComponent<Animator>();
		maxMovementSpeed = playerLocomotion.movementSpeed;
	}
    private void Start()
    {
		ResetScene.Instance.StorePlayer(gameObject, resetPos);
    }

    private void Update()
	{
		inputManager.HandleAllInputs();
	}

	private void FixedUpdate()
	{
		playerLocomotion.HandleAllMovement();
	}

	private void LateUpdate()
	{
		//if (!ResetScene.Instance.AllowInput) return;

		cameraManager.HandleAllCameraMovement();
		isInteracting = animator.GetBool("isInteracting");
		playerLocomotion.isJumping = animator.GetBool("isJumping");
		animator.SetBool("isGrounded", playerLocomotion.isGrounded);
		if (!playerLocomotion.isGrounded || playerLocomotion.isJumping)
		{
			playerLocomotion.movementSpeed = 3f;
		}
		else
		{
			playerLocomotion.movementSpeed = maxMovementSpeed;
		}
	}
}
