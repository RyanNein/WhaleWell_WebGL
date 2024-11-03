using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : NeinUtility.PersistentSingleton<Initializer>
{
	[SerializeField] string[] addativeSceneNames;
	[SerializeField] string nameOfFirstRoomScene;

	static bool hasInitialized = false;

	protected override void Awake()
	{
		if (hasInitialized)
		{
			Destroy(gameObject);
			return;
		}

		base.Awake();

		// Create addative (Management) Scnees. dont destroy on load
		foreach (var name in addativeSceneNames)
			SceneManager.LoadScene(name, LoadSceneMode.Additive);

		hasInitialized = true;
	}
}
