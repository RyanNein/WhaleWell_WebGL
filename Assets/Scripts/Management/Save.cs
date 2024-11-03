using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
	public string SceneID = string.Empty;
	public string SpawnID = string.Empty;

	public bool HasGivenFlowers = false;
	public bool HasGivenID = false;
	public bool HasFixedShip = false;
	public bool HasGivenPickaxe = false;
	public bool HasPickedUpShovel = false;
	public bool HasGivenMedkit = false;
	public bool HasFallen = false;
	public bool HasGivenPillow = false;
	public bool HasGivenLiveFish = false;
	public bool HasPickedUpMag = false;
	public bool HasGivenMag = false;
	public bool HasPickedUpPillow = false;
	public bool HasGivenMeat = false;
	public bool HasGivenPlant = false;
	public bool HasGivenLocket = false;
	public bool HasGivenDeadFish = false;
	public bool HasGivenFuel = false;
	public bool HasGivenOil = false;

	public List<string> DestroyedIDs = new List<string>();

	public List<InventoryItem> Inventory = new List<InventoryItem>();

	[System.Serializable]
	public class InventoryItem
	{
		public string ItemName;
		public int Quantity;

		public InventoryItem(string itemName, int quantity)
		{
			ItemName = itemName;
			Quantity = quantity;
		}
	}

	public Save (
		string _sceneID,
		string _spawnID,
		bool _hasGivenFlowers,
		bool _hasGivenID,
		bool _hasFixedShip,
		bool _hasGivenPickaxe,
		bool _hasPickedUpShovel,
		bool _hasGivenMedkit,
		bool _hasFallen,
		bool _hasGivenPillow,
		bool _hasGivenLiveFish,
		bool _hasPickedUpMag,
		bool _hasGivenMag,
		bool _hasPickedUpPillow,
		bool _hasGivenMeat,
		bool _hasGivenPlant,
		bool _hasGivenLocket,
		bool _hasGivenDeadFish,
		bool _hasGivenFuel,
		bool _hasGivenOil,
		List<string> _destroyedIDs,
		List<InventoryItem> _inventory
		)
	{
		SceneID = _sceneID;
		SpawnID = _spawnID;
		HasGivenFlowers = _hasGivenFlowers;
		HasGivenID = _hasGivenID;
		HasFixedShip = _hasFixedShip;
		HasGivenPickaxe = _hasGivenPickaxe;
		HasPickedUpShovel = _hasPickedUpShovel;
		HasGivenMedkit = _hasGivenMedkit;
		HasFallen = _hasFallen;
		HasGivenPillow = _hasGivenPillow;
		HasGivenLiveFish = _hasGivenLiveFish;
		HasPickedUpMag = _hasPickedUpMag;
		HasGivenMag = _hasGivenMag;
		HasPickedUpPillow = _hasPickedUpPillow;
		HasGivenMeat = _hasGivenMeat;
		HasGivenPlant = _hasGivenPlant;
		HasGivenLocket = _hasGivenLocket;
		HasGivenDeadFish = _hasGivenDeadFish;
		HasGivenFuel = _hasGivenFuel;
		HasGivenOil = _hasGivenOil;
		DestroyedIDs = _destroyedIDs;
		Inventory = _inventory;
	}
}
