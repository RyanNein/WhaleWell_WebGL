using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] GameObject firstMenu;
	GameObject currentMenuParent;

	[SerializeField] int buttonIndex = 0;
	List<IMenuButton> buttons = new List<IMenuButton>();

	PlayerInputAsset playerInputAsset;
	InputAction verticalAction;
	InputAction primaryInteractAction;

	float verticalInput;
	bool canVInput;

	private void Awake()
	{
		playerInputAsset = new PlayerInputAsset();
		verticalAction = playerInputAsset.PlayerMap.Vertical;
		primaryInteractAction = playerInputAsset.PlayerMap.PrimaryInteract;
	}

	private void OnEnable()
	{
		verticalAction.Enable();
		primaryInteractAction.Enable();
	}

	private void OnDisable()
	{
		verticalAction.Disable();
		primaryInteractAction.Disable();
	}

	private void Start()
	{
		NewMenu(firstMenu);
	}

	private void Update()
	{
		if (buttonIndex == -1)
			return;

		GetVerticalInput();

		if (verticalInput == 1f)
		{
			buttonIndex++;
			if (buttonIndex > buttons.Count - 1)
			{
				buttonIndex = 0;
			}
			HighlightButtons();
		}
		else if (verticalInput == -1f)
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
			buttonIndex = -1;
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
		canVInput = true;
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


	void GetVerticalInput()
	{
		var hInput = verticalAction.ReadValue<float>();
		if (canVInput)
		{
			if (!Mathf.Approximately(hInput, 0f))
			{
				verticalInput = Mathf.Round(hInput);
				canVInput = false;
				StartCoroutine(ResetCanVInput());
			}
		}
		else
		{
			verticalInput = 0f;
		}
	}

	IEnumerator ResetCanVInput()
	{
		yield return new WaitForSeconds(0.25f);
		canVInput = true;
	}
}
