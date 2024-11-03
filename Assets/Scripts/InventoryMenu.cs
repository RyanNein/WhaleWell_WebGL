using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryMenu : MonoBehaviour
{

	[SerializeField] GameObject displayItemPrefab;

	[SerializeField] Transform buttonSpawn;
	[SerializeField] GameObject panel;
		
	PlayerInputAsset playerInputAsset;
	InputAction secondaryInteractAction;

	private void Awake()
	{
		playerInputAsset = new PlayerInputAsset();
		secondaryInteractAction = playerInputAsset.PlayerMap.SecondaryInteract;
	}

	private void OnEnable()
	{
		secondaryInteractAction.Enable();
	}

	private void OnDisable()
	{
		secondaryInteractAction.Disable();
	}

	private void Start()
	{
		PopulateMenu();
	}

	private void Update()
	{
		if (secondaryInteractAction.WasPerformedThisFrame())
			InventoryManager.Instance.DestroyItemMenu();
	}

	void PopulateMenu()
	{
		var dic = InventoryManager.Instance.InventoryDictionary;

		int i = 0;
		foreach (KeyValuePair<SoItem, int> pair in dic)
		{
			var inst = Instantiate(displayItemPrefab, panel.transform);
			// inst.GetComponentInChildren<Image>().sprite = pair.Key.ItemSprite;
			inst.transform.Find("Item").GetComponent<Image>().sprite = pair.Key.ItemSprite;
			inst.GetComponentInChildren<TextMeshProUGUI>().text = pair.Value.ToString();

			i++;
		}

	}

}