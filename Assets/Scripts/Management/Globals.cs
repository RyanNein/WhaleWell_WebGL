using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : NeinUtility.PersistentSingleton<Globals>
{
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

	private void OnEnable()
	{
		SceneLoader.OnSceneStart += ImplementGlobals;
	}

	private void OnDisable()
	{
		SceneLoader.OnSceneStart -= ImplementGlobals;
	}

	public void AddDestroyable(string _ID)
	{
		DestroyedIDs.Add(_ID);
	}

	public void DestroyDestroyedObjects()
	{
		var destroyablesInScene = FindObjectsOfType<Destroyable>();
		foreach (var destroyableObject in destroyablesInScene)
		{
			if (DestroyedIDs.Contains(destroyableObject.ID))
			{
				destroyableObject.AlreadyDestroyed = true;
				Destroy(destroyableObject.gameObject);
			}
		}
	}

	void ImplementGlobals()
	{
		print(SceneLoader.Instance.CurrentScene);
		switch (SceneLoader.Instance.CurrentScene)
		{
			case "Hub":
				if (HasGivenID)
				{
					var factory = FindObjectOfType<Factory>();
					if (factory != null)
					{
						factory.CreateSceneTrigger();
					}
					var guard = FindObjectOfType<Guard>();
					if (guard != null)
					{
						guard.WarpToRight();
					}
				}
				if (HasFixedShip)
				{
					var ship = FindObjectOfType<Ship>();
					if (ship != null)
					{
						ship.SwitchToFixedShip();
					}
				}
				if (HasPickedUpShovel)
				{
					var raincoatMob = FindObjectOfType<RaincoatMob>();
					if (raincoatMob != null)
					{
						raincoatMob.SwitchToNoShovel();
					}
				}
				break;
			case "GreenHouse":
				if (HasGivenFlowers) 
				{
					var flowerBed = FindObjectOfType<FlowerBed>();
					if (flowerBed != null)
					{
						flowerBed.ActivateFlowers();
					}
				}
				break;
			case "Museum":
				if (HasGivenPickaxe)
				{
					if (HasGivenPickaxe)
					{
						var pickaxeDisplay = FindObjectOfType<PickaxeDisplay>();
						if (pickaxeDisplay != null)
						{
							pickaxeDisplay.ActivatePickaxe();
						}
					}
				}
				break;
			case "DeepFactory":
				if (HasGivenMedkit)
				{
					var miner = FindObjectOfType<Miner>();
					if (miner != null)
					{
						miner.SwitchToNoPickaxe();
					}
				}
				break;
			case "LakeArea":
				if (HasGivenPickaxe)
				{
					var whale = FindObjectOfType<Whale>();
					if (whale != null)
					{
						whale.ToggleBlood(false);
					}
				}
				break;
			default:
				break;
		}
	}
}
