using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[SelectionBase]
public class GameManager : NeinUtility.PersistentSingleton<GameManager>
{
	[SerializeField] GameObject PauseMenuPrefab;
	GameObject pauseMenuObject;

	public bool GameIsPaused => pauseMenuObject != null;
	
	Save saveData;
	
	private void Start()
	{
		LoadPlayerPrefs();
	}

	private void OnApplicationQuit()
	{
		SavePlayerPrefs();
	}

	#region PAUSE

	public void OpenPauseMenu()
	{
		if (GameIsPaused)
			return;

		pauseMenuObject = Instantiate(PauseMenuPrefab, transform);

		var player = FindObjectOfType<Player>();
		player.enabled = false;
	}

	public void DestroyPauseMenu(bool _enablePlayer)
	{
		if (!GameIsPaused)
			return;

		Destroy(pauseMenuObject);

		if (_enablePlayer)
		{
			var player = FindObjectOfType<Player>();
			player.enabled = true;
		}
	}

	#endregion

	#region MAIN MENU

	public void StartNewGame()
	{
		var emptySave = Resources.Load<TextAsset>("EmptySaveFile").text;
		UnpackSave(emptySave);

		SceneLoader.Instance.LoadScene("LakeArea");
	}

	public void ContinueGame()
	{
		string json;
		string path = Application.dataPath + "/SaveFile.json";

		if (File.Exists(path))
		{
			json = File.ReadAllText(path);
			UnpackSave(json);
			SceneLoader.Instance.LoadScene(saveData.SceneID, saveData.SpawnID);
		}
		else
		{
			StartNewGame();
		}
	}

	#endregion

	#region SAVE

	void LoadPlayerPrefs()
	{
		var masterVolume = PlayerPrefs.GetFloat("MasterVolume");
		AudioManager.Instance.SetGroupVolume(AudioManager.AudioGroups.MasterGroup, masterVolume);
	}

	public void SavePlayerPrefs()
	{
		var masterVolume = AudioManager.Instance.CurrentMasterVolume;
		PlayerPrefs.SetFloat("MasterVolume", masterVolume);
	}

	public void SaveGame()
	{
		var newSave = new Save(
			SceneLoader.Instance.CurrentScene,
			SceneLoader.Instance.LastSpawn,
			Globals.Instance.HasGivenFlowers,
			Globals.Instance.HasGivenID,
			Globals.Instance.HasFixedShip,
			Globals.Instance.HasGivenPickaxe,
			Globals.Instance.HasPickedUpShovel,
			Globals.Instance.HasGivenMedkit,
			Globals.Instance.HasFallen,
			Globals.Instance.HasGivenPillow,
			Globals.Instance.HasGivenLiveFish,
			Globals.Instance.HasPickedUpMag,
			Globals.Instance.HasGivenMag,
			Globals.Instance.HasPickedUpPillow,
			Globals.Instance.HasGivenMeat,
			Globals.Instance.HasGivenPlant,
			Globals.Instance.HasGivenLocket,
			Globals.Instance.HasGivenDeadFish,
			Globals.Instance.HasGivenFuel,
			Globals.Instance.HasGivenOil,
			Globals.Instance.DestroyedIDs,
			InventoryManager.Instance.InventoryList
			);

		var json = JsonUtility.ToJson(newSave);
		print(json.ToString());
		File.WriteAllText(Application.dataPath + "/SaveFile.json", json);
	}
	
	public void UnpackSave(string _json)
	{
		saveData = JsonUtility.FromJson<Save>(_json);

		Globals.Instance.HasGivenFlowers = saveData.HasGivenFlowers;
		Globals.Instance.HasGivenID = saveData.HasGivenID;
		Globals.Instance.HasFixedShip = saveData.HasFixedShip;
		Globals.Instance.HasGivenPickaxe = saveData.HasGivenPickaxe;
		Globals.Instance.HasPickedUpShovel = saveData.HasPickedUpShovel;
		Globals.Instance.HasGivenMedkit = saveData.HasGivenMedkit;
		Globals.Instance.HasFallen = saveData.HasFallen;
		Globals.Instance.HasGivenPillow = saveData.HasGivenPillow;
		Globals.Instance.HasGivenLiveFish = saveData.HasGivenLiveFish;
		Globals.Instance.HasPickedUpMag = saveData.HasPickedUpMag;
		Globals.Instance.HasGivenMag = saveData.HasGivenMag;
		Globals.Instance.HasPickedUpPillow = saveData.HasPickedUpPillow;
		Globals.Instance.HasGivenMeat = saveData.HasGivenMeat;
		Globals.Instance.HasGivenPlant = saveData.HasGivenPlant;
		Globals.Instance.HasGivenLocket = saveData.HasGivenLocket;
		Globals.Instance.HasGivenDeadFish = saveData.HasGivenDeadFish;
		Globals.Instance.HasGivenFuel = saveData.HasGivenFuel;
		Globals.Instance.HasGivenOil = saveData.HasGivenOil;

		Globals.Instance.DestroyedIDs = saveData.DestroyedIDs;

		InventoryManager.Instance.LoadInventory(saveData.Inventory);
	}

	public void EraseSave()
	{
		string path = Application.dataPath + "/SaveFile.json";

		if (File.Exists(path))
		{
			File.Delete(path);
			Debug.Log("Save file erased.");
		}
		else
		{
			Debug.LogWarning("No save file found to erase.");
		}
	}

	#endregion
}
