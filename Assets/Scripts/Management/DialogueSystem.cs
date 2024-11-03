using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueSystem : NeinUtility.PersistentSingleton<DialogueSystem>
{

	[SerializeField] GameObject lineView;
	[SerializeField] TextMeshProUGUI lineViewText;

	[SerializeField] GameObject optionsView;
	[SerializeField] GameObject optionsParent;

	[SerializeField] GameObject optionPrefab;

	[SerializeField] GameObject container;
	[SerializeField] Vector3 topPosition;
	[SerializeField] Vector3 bottomPosition;

	List<GameObject> optionObjects = new List<GameObject>();

	[SerializeField] Image portrait;

	GameObject sourceObject;
	bool returnToMain;

	int pageIndex;
	int optionIndex;

	SoStory currentStory;

	public bool isDialogueActive;
	public bool canStartDialogue = true;
	bool cancontinue = false;

	PlayerInputAsset playerInputAsset;
	InputAction verticalAction;
	InputAction primaryInteractAction;

	float verticalInput;
	bool canVInput;

	public delegate void DialogueEvents();
	public static event DialogueEvents
		OnDialogueStart,
		OnDialogueEnd;

	protected override void Awake()
	{
		base.Awake();

		playerInputAsset = new PlayerInputAsset();
		verticalAction = playerInputAsset.PlayerMap.Vertical;
		primaryInteractAction = playerInputAsset.PlayerMap.PrimaryInteract;
	}

	private void OnEnable()
	{
		verticalAction.Enable();
		primaryInteractAction.Enable();
		SceneLoader.OnSceneStart += EndDialogue;
	}

	private void OnDisable()
	{
		verticalAction.Disable();
		primaryInteractAction.Disable();
		SceneLoader.OnSceneStart -= EndDialogue;
	}

	private void Update()
	{
		if (isDialogueActive)
		{
			// line view:
			if (currentStory.Pages[pageIndex].options.Length == 0)
			{
				// next page button
				if (isDialogueActive && cancontinue)
				{
					if (primaryInteractAction.WasPerformedThisFrame())
					{
						NextPage();
					}
				}
			}
			else
			// choose option
			{
				GetVerticalInput();

				// choose up and down option:
				if (verticalInput == 1f)
				{
					optionIndex++;
					if (optionIndex > currentStory.Pages[pageIndex].options.Length - 1)
					{
						optionIndex = 0;
					}
					ColorOptions();
				}
				else if (verticalInput == -1f)
				{
					optionIndex--;
					if (optionIndex < 0)
					{
						optionIndex = currentStory.Pages[pageIndex].options.Length - 1;
					}
					ColorOptions();
				}

				// click option:
				if (primaryInteractAction.WasPerformedThisFrame() && cancontinue)
				{
					var nextPageIndex = currentStory.Pages[pageIndex].options[optionIndex].nextPage;
					print(nextPageIndex);
					NextPage(nextPageIndex);
				}
			}
		}
	}

	public void StartDialogue(SoStory _story, GameObject _sourceObject = null, bool _returnToMain = true)
	{
		if (!canStartDialogue) return;

		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			var playerTransform = player.transform;

			// Check if the player is on the bottom half of the screen
			Vector3 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
			RectTransform containerRectTransform = container.GetComponent<RectTransform>();
			if (screenPos.y < Screen.height / 2)
			{
				containerRectTransform.anchoredPosition = topPosition;
			}
			else
			{
				containerRectTransform.anchoredPosition = bottomPosition;
			}

			// transition player state
			player.StateChange(Player.States.None);
		}

		currentStory = _story;
		sourceObject = _sourceObject;
		returnToMain = _returnToMain;
		portrait.sprite = null;
		pageIndex = -1;
		NextPage();
		isDialogueActive = true;
		canStartDialogue = false;
		OnDialogueStart?.Invoke();
	}

	void EndDialogue()
	{
		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			if (returnToMain)
			{
				player.StateChange(Player.States.Main);
			}
		}

		lineView.SetActive(false);
		optionsView.SetActive(false);
		isDialogueActive = false;
		OnDialogueEnd?.Invoke();
		StartCoroutine(ResetCanStartDialogue());
	}

	void NextPage(int _nextPageIndex = -1)
	{
		// check for last page field:
		if (pageIndex != -1)
		{
			if (currentStory.Pages[pageIndex].IsLastPage)
			{
				EndDialogue();
				return;
			}
		}

		optionObjects.Clear();

		if (_nextPageIndex == -1)
		{
			pageIndex++;
		}
		else
		{
			pageIndex = _nextPageIndex;
		}

		// check for actual last page:
		if (pageIndex > currentStory.Pages.Length - 1)
		{
			EndDialogue();
			return;
		}

		var currentPage = currentStory.Pages[pageIndex];
		// line view:
		if (currentPage.options.Length == 0)
		{
			lineView.SetActive(true);
			optionsView.SetActive(false);
			lineViewText.text = currentPage.text;
			if (currentPage.Portrait != null)
			{
				portrait.sprite = currentPage.Portrait;
			}
			if (currentPage.methodName != string.Empty)
			{
				Invoke(currentPage.methodName, 0f);
			}
		}
		// options view:
		else
		{
			lineView.SetActive(false);
			optionsView.SetActive(true);

			foreach (Transform child in optionsParent.transform)
			{
				Destroy(child.gameObject);
			}

			foreach (var option in currentPage.options)
			{
				var newOption = Instantiate(optionPrefab, optionsParent.transform);
				var textComponent = newOption.GetComponent<TextMeshProUGUI>();
				textComponent.text = option.text;
				optionObjects.Add(newOption);
			}

			ColorOptions();
		}

		cancontinue = false;
		canVInput = true;
		StartCoroutine(ResetCanContinue());
	}

	IEnumerator ResetCanStartDialogue()
	{
		yield return new WaitForSeconds(0.1f);
		canStartDialogue = true;
	}

	IEnumerator ResetCanContinue()
	{
		yield return new WaitForSeconds(0.1f);
		cancontinue = true;
	}

	void ColorOptions()
	{
		foreach (var option in optionObjects)
		{
			option.GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0.5f);
		}
		optionObjects[optionIndex].GetComponent<TextMeshProUGUI>().color = Color.black;
	}

	void GetVerticalInput()
	{
		var vInput = verticalAction.ReadValue<float>();
		if (canVInput)
		{
			if (!Mathf.Approximately(vInput, 0f))
			{
				verticalInput = Mathf.Round(vInput);
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
		yield return new WaitForSeconds(0.5f);
		canVInput = true;
	}

	#region STORY METHODS

	[SerializeField] SoItem flowerItemSo;
	[SerializeField] SoItem deadFishItemSo;
	[SerializeField] SoItem shovelItemSo;
	[SerializeField] SoItem magItemSo;
	[SerializeField] SoItem featherItemSo;
	[SerializeField] SoItem idItemSo;
	[SerializeField] SoItem oilItemSo;
	[SerializeField] SoItem dollarItemSo;
	[SerializeField] SoItem pillowItemSo;
	[SerializeField] SoItem liveFishItemSo;
	[SerializeField] SoItem meatItemSo;
	[SerializeField] SoItem hammerItemSo;
	[SerializeField] SoItem fuelItemSo;
	[SerializeField] SoItem plantItemSo;
	[SerializeField] SoItem medkitItemSo;
	[SerializeField] SoItem locketItemSo;
	[SerializeField] SoItem pickaxeItemSo;
	[SerializeField] SoItem contractItemSo;

	[SerializeField] AudioClip whaleClip;

	#region PICKUP


	void PickupFlower()
	{
		InventoryManager.Instance.AddItemToInventory(flowerItemSo);
		var destroyable = sourceObject.GetComponent<Destroyable>();
		if (destroyable != null)
		{
			destroyable.PermanentDestroy();
		}
	}

	void PickupDeadFish()
	{
		InventoryManager.Instance.AddItemToInventory(deadFishItemSo);
		var destroyable = sourceObject.GetComponent<Destroyable>();
		if (destroyable != null)
		{
			destroyable.PermanentDestroy();
		}
	}
	void PickupFeather()
	{
		InventoryManager.Instance.AddItemToInventory(featherItemSo);
		var destroyable = sourceObject.GetComponent<Destroyable>();
		if (destroyable != null)
		{
			destroyable.PermanentDestroy();
		}
	}

	void PickupShovel()
	{
		InventoryManager.Instance.AddItemToInventory(shovelItemSo);
		Globals.Instance.HasPickedUpShovel = true;
		var raincoatMob = FindObjectOfType<RaincoatMob>();
		if (raincoatMob != null)
		{
			raincoatMob.SwitchToNoShovel();
		}
	}

	void PickupMag()
	{
		InventoryManager.Instance.AddItemToInventory(magItemSo);
		Globals.Instance.HasPickedUpMag = true;
		var destroyable = sourceObject.GetComponent<Destroyable>();
		if (destroyable != null)
		{
			destroyable.PermanentDestroy();
		}
	}


	void PickupID()
	{
		InventoryManager.Instance.AddItemToInventory(idItemSo);
	}

	void PickupOil()
	{
		InventoryManager.Instance.AddItemToInventory(oilItemSo);
	}

	void PickupDollar()
	{
		InventoryManager.Instance.AddItemToInventory(dollarItemSo);
	}

	void PickupPillow()
	{
		InventoryManager.Instance.AddItemToInventory(pillowItemSo);
		Globals.Instance.HasPickedUpPillow = true;
	}

	void PickupLiveFish()
	{
		InventoryManager.Instance.AddItemToInventory(liveFishItemSo);
	}

	void PickupMeat()
	{
		InventoryManager.Instance.AddItemToInventory(meatItemSo);
	}

	void PickupHammer()
	{
		InventoryManager.Instance.AddItemToInventory(hammerItemSo);
	}

	void PickupFuel()
	{
		InventoryManager.Instance.AddItemToInventory(fuelItemSo);
	}

	void PickupPlant()
	{
		InventoryManager.Instance.AddItemToInventory(plantItemSo);
	}

	void PickupLocket()
	{
		InventoryManager.Instance.AddItemToInventory(locketItemSo);
	}

	void PickupMedkit()
	{
		InventoryManager.Instance.AddItemToInventory(medkitItemSo);
	}

	void PickupPickaxe()
	{
		InventoryManager.Instance.AddItemToInventory(pickaxeItemSo);
		Globals.Instance.HasGivenMedkit = true;
		var miner = FindObjectOfType<Miner>();
		if (miner != null)
		{
			miner.SwitchToNoPickaxe();
		}
	}

	void PickupContract()
	{
		InventoryManager.Instance.AddItemToInventory(contractItemSo);
	}

	#endregion

	#region REMOVE
	
	void RemoveMag()
	{
		InventoryManager.Instance.RemoveItemFromInventory(magItemSo);
		Globals.Instance.HasGivenMag = true;
	}

	void RemoveOil()
	{
		InventoryManager.Instance.RemoveItemFromInventory(oilItemSo);
		Globals.Instance.HasGivenOil = true;
	}

	void RemoveFeathers()
	{
		InventoryManager.Instance.RemoveItemFromInventory(featherItemSo, 4);
	}

	void RemovePillow()
	{
		InventoryManager.Instance.RemoveItemFromInventory(pillowItemSo);
		Globals.Instance.HasGivenPillow = true;
	}

	void RemoveDollar()
	{
		InventoryManager.Instance.RemoveItemFromInventory(dollarItemSo);
	}

	void RemoveMeat()
	{
		InventoryManager.Instance.RemoveItemFromInventory(meatItemSo);
		Globals.Instance.HasGivenMeat = true;
	}

	void RemoveDeadFish()
	{
		InventoryManager.Instance.RemoveItemFromInventory(deadFishItemSo, 6);
		Globals.Instance.HasGivenDeadFish = true;
	}

	void RemoveFlowers()
	{
		InventoryManager.Instance.RemoveItemFromInventory(flowerItemSo, 3);
		Globals.Instance.HasGivenFlowers = true;
		var flowerBed = FindObjectOfType<FlowerBed>();
		if (flowerBed != null)
		{
			flowerBed.ActivateFlowers();
		}

	}

	void RemoveFuel()
	{
		InventoryManager.Instance.RemoveItemFromInventory(fuelItemSo);
		Globals.Instance.HasGivenFuel = true;
	}

	void RemovePlant()
	{
		InventoryManager.Instance.RemoveItemFromInventory(plantItemSo);
		Globals.Instance.HasGivenPlant = true;
	}

	void RemoveLiveFish()
	{
		InventoryManager.Instance.RemoveItemFromInventory(liveFishItemSo);
		Globals.Instance.HasGivenLiveFish = true;
	}

	void RemoveLocket()
	{
		InventoryManager.Instance.RemoveItemFromInventory(locketItemSo);
		Globals.Instance.HasGivenLocket = true;
	}

	void RemoveHammer()
	{
		InventoryManager.Instance.RemoveItemFromInventory(hammerItemSo);
		Globals.Instance.HasFixedShip = true;
		var ship = FindObjectOfType<Ship>();
		if (ship != null)
		{
			ship.SwitchToFixedShip();
		}
	}

	void RemoveMedkit()
	{
		InventoryManager.Instance.RemoveItemFromInventory(medkitItemSo);
	}

	void RemovePickaxe()
	{
		InventoryManager.Instance.RemoveItemFromInventory(pickaxeItemSo);
		Globals.Instance.HasGivenPickaxe = true;
		var pickaxeDisplay = FindObjectOfType<PickaxeDisplay>();
		if (pickaxeDisplay != null)
		{
			pickaxeDisplay.ActivatePickaxe();
		}
	}

	void RemoveContract()
	{
		InventoryManager.Instance.RemoveItemFromInventory(contractItemSo);
	}

	#endregion

	void MoveGuard()
	{
		var guard = FindObjectOfType<Guard>();
		if (guard != null)
		{
			// guard.WarpToRight();
			guard.WalkToRight();
		}

		var factory = FindObjectOfType<Factory>();
		if (factory != null)
		{
			factory.CreateSceneTrigger();
		}

		Globals.Instance.HasGivenID = true;
	}

	void PlayWhaleNoise()
	{
		AudioManager.Instance.PlaySFXOneShot(whaleClip);
	}

	#region SCENE CHANGE

	void SceneFall()
	{
		SceneLoader.Instance.LoadScene("Fall");
		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			player.enabled = false;
		}
	}

	void SceneSquidFall()
	{
		SceneLoader.Instance.LoadScene("SquidFall");
		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			player.enabled = false;
		}
	}

	void SceneEndScreen()
	{
		SceneLoader.Instance.LoadScene("EndScreen");
		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			player.enabled = false;
		}
	}

	#endregion

	#endregion
}
