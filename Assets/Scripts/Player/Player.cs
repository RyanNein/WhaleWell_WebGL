using System;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	// State:
	public enum States : short { None, Main, Falling }
	[SerializeField] States state = States.None;
	public States State => state;
	[SerializeField] States previousState = States.None;
	public States PreviousState => previousState;
	
	Action updateAction;


	// Movement vars:
	[SerializeField] float speed;
	[SerializeField] float fallingSpeed;
	Vector2 _lookDirection = Vector2.right;
	public Vector2 LookDirection 
	{
		get
		{
			if (State == States.Falling)
				return Vector2.down;
			else
				return _lookDirection;
		}
	}
	Vector2 moveDirection;


	[SerializeField] bool isOnWater;
	public bool IsOnWater
	{
		get { return isOnWater; }
		set 
		{ 
			isOnWater = value;
			myAnimator.SetBool("isSwimming", value);
		}
	}
	

	// Quack:
	[SerializeField] GameObject quackPrefab;
	[SerializeField] AudioClip quackClip;
	float quackDistance = 2;


	// Cache:
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] SpriteRenderer shadowSpriteRenderer;
	[SerializeField] Animator myAnimator;
	[SerializeField] Rigidbody2D myRigidBody;


	public int CurretnSortOrder => mySpriteRenderer.sortingOrder;

	// Input:
	PlayerInputAsset playerInputAsset;
	InputAction horizontalAction;
	InputAction verticalAction;
	InputAction primaryInteractAction;
	InputAction secondaryInteractAction;
	InputAction MenuButton;

	#region LOOP:

	private void Awake()
	{
		updateAction = MainUpdate;
		StateChange(States.Main);
		
		// Input Cache:
		playerInputAsset = new PlayerInputAsset();
		horizontalAction = playerInputAsset.PlayerMap.Horizontal;
		verticalAction = playerInputAsset.PlayerMap.Vertical;
		primaryInteractAction = playerInputAsset.PlayerMap.PrimaryInteract;
		secondaryInteractAction = playerInputAsset.PlayerMap.SecondaryInteract;
		MenuButton = playerInputAsset.PlayerMap.MenuButton;
	}

	private void OnEnable()
	{
		// Input Subscribe
		horizontalAction.Enable();
		verticalAction.Enable();
		primaryInteractAction.Enable();
		secondaryInteractAction.Enable();
		MenuButton.Enable();
	}

	private void OnDisable()
	{
		// Input Unsubscribe
		horizontalAction.Disable();
		verticalAction.Disable();
		primaryInteractAction.Disable();
		secondaryInteractAction.Disable();
		MenuButton.Disable();
	}

	private void Update()
	{
		updateAction();
	}

	private void FixedUpdate()
	{
		var currentPosition = myRigidBody.position;
		var movePosition = (moveDirection * speed * Time.fixedDeltaTime) + currentPosition;

		if (State == States.Falling)
			movePosition.y -= fallingSpeed * Time.fixedDeltaTime;

		myRigidBody.MovePosition(movePosition);
	}

	#endregion


	#region MAIN STATE:

	private void MainInit()
	{
		updateAction = MainUpdate;
		myAnimator.SetBool("isFalling", false);
	}

	private void MainUpdate()
	{
		var currentInput = HandleInputs();
		moveDirection = Vector2.zero;


		// --------------------------------- MOVEMENT ----------------------------------\\
		if (!Mathf.Approximately(currentInput.x, 0f) || !Mathf.Approximately(currentInput.y, 0f))
		{
			myAnimator.SetBool("isMoving", true);

			if (currentInput.x > 0)
				moveDirection = _lookDirection = Vector2.right;
			else if (currentInput.x < 0)
				moveDirection = _lookDirection = Vector2.left;
			else if (currentInput.y > 0)
				moveDirection = _lookDirection = Vector2.up;
			else if (currentInput.y < 0)
				moveDirection = _lookDirection = Vector2.down;

			HandleFlip();
		}
		else
		{
			myAnimator.SetBool("isMoving", false);
		}


		// ----------------------------------- INTERACTION BUTTONS ----------------------------------\\
		if (MenuButton.WasPerformedThisFrame())
		{
			GameManager.Instance.OpenPauseMenu();
		}
		else if (primaryInteractAction.WasPerformedThisFrame())
		{
			Vector2 boxSize = new Vector2(0.5f, 0.5f); // Define the size of the box
			Vector2 boxOrigin = (Vector2)transform.position + Vector2.up * 0.5f + _lookDirection * 0.25f;
			float boxDistance = 0.5f;

			RaycastHit2D[] hits = Physics2D.BoxCastAll(boxOrigin, boxSize, 0f, _lookDirection, boxDistance, ~LayerMask.GetMask("Ignore Raycast"));
			Debug.DrawRay(boxOrigin, _lookDirection * boxDistance, Color.red, 1);

			bool interactableHit = false;
			foreach (var hit in hits)
			{
				var hitObject = hit.collider.gameObject;
				Debug.Log("hit " + hitObject.name);

				// Skip triggers
				if (hit.collider.isTrigger)
				{
					continue;
				}

				IInteractable interactable = hitObject.GetComponent<IInteractable>();
				if (interactable != null)
				{
					print("hit interactive");
					interactable.PlayerInteraction();
					interactableHit = true;
				}
			}

			if (!interactableHit)
			{
				// QUACK:
				var direction = mySpriteRenderer.flipX == true ? Vector2.left : Vector2.right;
				// var spawnLocation = (Vector2)transform.position + direction * quackDistance;
				var spawnLocation = new Vector2(transform.position.x, transform.position.y + 0.25f) + direction * quackDistance;

				Instantiate(quackPrefab, spawnLocation, Quaternion.identity, transform);
				AudioManager.Instance.PlaySFXOneShot(quackClip);
			}
		}
		else if (secondaryInteractAction.WasPerformedThisFrame())
		{
			InventoryManager.Instance.OpenInventoryMenu();
		}

	}

	private void MainExit()
	{
		moveDirection = Vector2.zero;
		myAnimator.SetBool("isMoving", false);
	}
	#endregion


	#region FALLING STATE:

	private void FallingInit()
	{
		updateAction = FallingUpdate;
		myAnimator.SetBool("isFalling", true);
	}

	private void FallingUpdate()
	{
		var currentInput = HandleInputs();
		_lookDirection = moveDirection = Vector2.down;

		if (currentInput.x > 0)
			moveDirection = Vector2.right;
		else if (currentInput.x < 0)
			moveDirection = Vector2.left;

		HandleFlip();
	}

	private void FallingExit()
	{
		moveDirection = Vector2.zero;
		myAnimator.SetBool("isMoving", false);
	}

	#endregion


	#region NONE STATE:

	private void NoneInit()
	{
		updateAction = NoneUpdate;
	}

	private void NoneUpdate()
	{
	}

	private void NoneExit()
	{

	}

	#endregion


	#region STATE MACHINE:

	public void StateChange(States newState)
	{
		if (State == newState)
		{
			Debug.LogWarning("attempted to change to current state");
			return;
		}

		previousState = State;
		state = newState;

		switch (PreviousState)
		{
			case States.Main:
				MainExit();
				break;
			case States.Falling:
				FallingExit();
				break;
			case States.None:
				NoneExit();
				break;
		}

		switch (State)
		{
			case States.Main:
				MainInit();
				break;
			case States.Falling:
				FallingInit();
				break;
			case States.None:
				NoneInit();
				break;
		}
	}

	#endregion


	public void ChangeDepthLayer(int _newLayer, int _newSortingLayer)
	{
		gameObject.layer = _newLayer;
		mySpriteRenderer.sortingOrder = _newSortingLayer;
	}

	#region UTILITY:
	private Vector2 HandleInputs()
	{
		var hInput = horizontalAction.ReadValue<float>();
		var vInput = verticalAction.ReadValue<float>();
		return new Vector2(hInput, vInput);
	}

	private void HandleFlip()
	{
		if (moveDirection.x != 0)
		{
			mySpriteRenderer.flipX = moveDirection.x < 0;
			shadowSpriteRenderer.flipX = moveDirection.x < 0;
		}
	}

	#endregion
}
