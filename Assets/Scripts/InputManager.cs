using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : Singleton<InputManager>
{
	private PlayerControls playerControls;
	private AnimatorManager animatorManager;
	private PlayerLocomotion playerLocomotion;
	
	private Vector2 movementInput;
	private Vector2 cameraInput;

	public float cameraXInput;
	public float cameraYInput;

	public float verticalInput;
	public float horizontalInput;
	
	private float moveAmount;

	public bool jumpInput;

	protected override void Awake()
	{
		base.Awake();
		animatorManager = GetComponent<AnimatorManager>();
		playerLocomotion = GetComponent<PlayerLocomotion>();
	}
    private void Start()
    {
        ResetScene.Instance.StorePos = transform.position;
    }

    private void OnEnable()
	{
		if (playerControls == null)
		{
			playerControls = new PlayerControls();
			playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
			playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
			playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
		}
		playerControls.Enable();
	}

	private void OnDisable()
	{
		playerControls.Disable();
	}

	public void HandleAllInputs()
	{
		HandleMovement();
		HandleJumpingInput();
	}
	
	private void HandleMovement()
	{
		switch (playerLocomotion.isJumping)
		{
			case true:
				//if(isGrounded)
                verticalInput = movementInput.y;
                //cameraXInput = cameraInput.x;
                //cameraYInput = cameraInput.y;
                break;
			default: break;
		}
                horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        cameraXInput = cameraInput.x;
        cameraYInput = cameraInput.y;
        animatorManager.UpdateAnimatorValues(0, moveAmount);
	}
	public void ResetMoveInput()
	{
		verticalInput = 0;
		//horizontalInput = 0;
	}
	public void ForceJump()
	{
		jumpInput = true;
		HandleJumpingInput();
    }
	public void HandleJumpingInput()
	{
		switch (jumpInput)
		{
			case true:
                if (!playerLocomotion.isJumping)
                {
                    jumpInput = false;
                    playerLocomotion.HandleJumping();
                }
                break;
			default: break;
		}
	}
}
