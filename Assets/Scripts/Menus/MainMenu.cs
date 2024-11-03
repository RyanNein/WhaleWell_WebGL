using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
	[SerializeField] GameObject firstMenu;
	GameObject currentMenuParent;
	
	List<IMenuButton> buttons = new List<IMenuButton>();
	public int buttonIndex = 0;

	PlayerInputAsset playerInputAsset;
	InputAction horizontalAction;
	InputAction primaryInteractAction;
	InputAction secondaryInteractAction;

	float horizontalInput;
	bool canHInput;

	private void Awake()
	{
		playerInputAsset = new PlayerInputAsset();
		horizontalAction = playerInputAsset.PlayerMap.Horizontal;
		primaryInteractAction = playerInputAsset.PlayerMap.PrimaryInteract;
		secondaryInteractAction = playerInputAsset.PlayerMap.SecondaryInteract;
	}

	private void OnEnable()
	{
		horizontalAction.Enable();
		primaryInteractAction.Enable();
		secondaryInteractAction.Enable();
	}

	private void OnDisable()
	{
		horizontalAction.Disable();
		primaryInteractAction.Disable();
		secondaryInteractAction.Disable();
	}

	private void Start()
	{
		NewMenu(firstMenu);
	}

	private void Update()
	{
		if (buttonIndex == -1)
			return;

		GetHorizontalInput();

		if (horizontalInput == 1f)
		{
			buttonIndex++;
			if (buttonIndex > buttons.Count - 1)
			{
				buttonIndex = 0;
			}
			HighlightButtons();
		}
		else if (horizontalInput == -1f)
		{
			buttonIndex--;
			if (buttonIndex < 0)
			{
				buttonIndex = buttons.Count - 1;
			}
			HighlightButtons();
		}

		if (primaryInteractAction.WasPerformedThisFrame())
		{
			buttons[buttonIndex].DoClick();
		}

		if (secondaryInteractAction.WasPerformedThisFrame())
		{
			NewMenu(firstMenu);
		}
	}

	public void NewMenu(GameObject _buttonParent)
	{
		if (currentMenuParent != null)
		{
			currentMenuParent.SetActive(false);
		}

		buttons.Clear();
		buttonIndex = 0;
		currentMenuParent = _buttonParent;

		foreach (Transform child in _buttonParent.transform)
		{
			IMenuButton menuButton = child.GetComponent<IMenuButton>();
			if (menuButton != null)
			{
				buttons.Add(menuButton);
			}
		}

		_buttonParent.SetActive(true);
		canHInput = true;
		HighlightButtons();
	}

	void HighlightButtons()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			var toOn = i == buttonIndex ? true : false;
			buttons[i].Highlight(toOn);
		}
	}


	void GetHorizontalInput()
	{
		var hInput = horizontalAction.ReadValue<float>();
		if (canHInput)
		{
			if (!Mathf.Approximately(hInput, 0f))
			{
				horizontalInput = Mathf.Round(hInput);
				canHInput = false;
				StartCoroutine(ResetCanHInput());
			}
		}
		else
		{
			horizontalInput = 0f;
		}
	}

	IEnumerator ResetCanHInput()
	{
		yield return new WaitForSeconds(0.25f);
		canHInput = true;
	}
}
