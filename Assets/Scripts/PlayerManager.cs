using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
	private InputManager inputManager;
	private CameraManager cameraManager;
	private PlayerLocomotion playerLocomotion;
	private Animator animator;
	private float maxMovementSpeed;
	public bool isInteracting;
	[SerializeField] Transform resetPos;
	ResetScene rs;
    bool isShow = false;
    [HideInInspector] public int curIndex = 0;
    protected override void Awake()
	{
		base.Awake();
		inputManager = GetComponent<InputManager>();
		//cameraManager = FindObjectOfType<CameraManager>();
		cameraManager = FindFirstObjectByType<CameraManager>();
		playerLocomotion = GetComponent<PlayerLocomotion>();
		animator = GetComponent<Animator>();
		maxMovementSpeed = playerLocomotion.movementSpeed;
	}
    private void Start()
    {
        isShow = false;
        rs = ResetScene.Instance;
		ResetScene.Instance.StorePlayer(gameObject, resetPos);
    }

    private void Update()
	{
        inputManager.HandleAllInputs();
	}

	private void FixedUpdate()
	{
        switch (rs.AllowInput)
        {
            case true:
                playerLocomotion.HandleAllMovement();
                break;
            default: break;
        }
	}

	private void LateUpdate()
	{
        //if (!ResetScene.Instance.AllowInput) return;
        cameraManager.HandleAllCameraMovement();
        //switch (rs.AllowInput)
        //{
        //    case true:
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
        //        break;
        //    default: break;
        //}
	}
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Quiz")
        {
            if (isShow) return;
            isShow = true;
            ResetScene.Instance.AllowInput = false;
            Debug.LogError("Show Quiz");
            ResetScene.Instance.ShowQuizPanel(true);
        }
    }
}
