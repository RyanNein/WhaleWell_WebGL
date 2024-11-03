using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : NeinUtility.PersistentSingleton<InventoryManager>
{

	[SerializeField] GameObject itemMenuPrefab;
	GameObject activeMenuObject;

	[SerializeField] List<SoItem> allItems = new List<SoItem>();

	Dictionary<SoItem, int> inventoryDictionary = new Dictionary<SoItem, int>();
	public Dictionary<SoItem, int> InventoryDictionary 
	{
		get => inventoryDictionary;
		private set => inventoryDictionary = value;
	}

	public delegate void ItemMenuEvents();
	public static event ItemMenuEvents
		OnMenuOpen,
		OnMenuClose;


	public List<Save.InventoryItem> InventoryList
	{
		get
		{
			var inventoryItems = new List<Save.InventoryItem>();

			foreach (var item in InventoryDictionary)
			{
				inventoryItems.Add(new Save.InventoryItem(item.Key.ItemName, item.Value));
			}

			return inventoryItems;
		}
	}

public void AddItemToInventory(SoItem _soItem)
	{
		SoItem so = _soItem;
		if (InventoryDictionary.ContainsKey(so))
		{
			// add to key
			InventoryDictionary[so]++;
		}
		else
		{
			// make new key
			InventoryDictionary.Add(so, 1);
		}

		Debug.Log(so.name + ": " + InventoryDictionary[so]);
	}

	public void RemoveItemFromInventory(SoItem _soItem, int _amount = 1)
	{
		SoItem so = _soItem;

		for (int i = 0; i < _amount; i++)
		{
			if (InventoryDictionary.ContainsKey(so))
			{
				InventoryDictionary[so]--;
				if (InventoryDictionary[so] <= 0)
				{
					InventoryDictionary.Remove(so);
				}
				Debug.Log(so.name + " removed: " + (InventoryDictionary.ContainsKey(so) ? InventoryDictionary[so].ToString() : "0"));
			}
			else
			{
				Debug.LogWarning("Attempted to remove an item that is not in the inventory: " + so.name);
			}
		}
	}

	public bool HasItem(SoItem _item)
	{
		if (InventoryDictionary.ContainsKey(_item))
		{
			if (InventoryDictionary[_item] > 0)
				return true;
		}
		return false;
	}

	public int NumberOfItem(SoItem _item)
	{
		if (InventoryDictionary.ContainsKey(_item))
		{
			return InventoryDictionary[_item];
		}
		else
		{
			return 0;
		}
	}

	public void OpenInventoryMenu()
	{
		if (activeMenuObject != null)
		{
			Debug.LogWarning("Attempted to make two inventory menus");
			return;
		}

		activeMenuObject = Instantiate(itemMenuPrefab, transform);

		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			player.StateChange(Player.States.None);
		}

		OnMenuOpen?.Invoke();
	}

	public void DestroyItemMenu()
	{
		Destroy(activeMenuObject);
		
		var player = FindObjectOfType<Player>();
		if (player != null)
		{
			player.StateChange(Player.States.Main);
		}
		
		OnMenuClose?.Invoke();
	}

	public SoItem GetItemByName(string itemName)
	{
		foreach (var item in allItems)
		{
			if (item.ItemName == itemName)
			{
				return item;
			}
		}

		Debug.LogWarning("Item not found: " + itemName);
		return null;
	}

	public void LoadInventory(List<Save.InventoryItem> _inventoryList)
	{
		InventoryDictionary.Clear();
		
		foreach (var item in _inventoryList)
		{
			SoItem soItem = GetItemByName(item.ItemName);
			if (soItem != null)
			{
				InventoryDictionary[soItem] = item.Quantity;
			}
			else
			{
				Debug.LogWarning("Failed to load item: " + item.ItemName);
			}
		}
	}
}