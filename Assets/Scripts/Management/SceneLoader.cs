using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NeinUtility.PersistentSingleton<SceneLoader>
{
	public float fadeOutTimeInSeconds = 1f;
	[SerializeField] Animator sceneFadeAnimator;

	private bool isFading;
	public bool IsFading
	{
		get { return isFading; }
		private set { isFading = value; }
	}

	public string CurrentScene => SceneManager.GetActiveScene().name;
	public string LastSpawn = string.Empty;


	public delegate void SceneEvents();
	public static event SceneEvents
		OnSceneStart,
		OnSceneFadeOutStart,
		OnSceneFadeInEnd;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void LoadScene(string _sceneName, string _spawnID = null)
	{
		StartCoroutine(ExitSceneTransition(_sceneName, _spawnID));
	}

	IEnumerator ExitSceneTransition(string _sceneName, string _spawnID)
	{
		sceneFadeAnimator.Play("CrossfadeExit");
		IsFading = true;
		OnSceneFadeOutStart?.Invoke();
		yield return new WaitForSeconds(fadeOutTimeInSeconds);

		SceneManager.LoadScene(_sceneName);

		yield return new WaitForSeconds(0.1f);

		// move player to spawn location:
		var spawns = FindObjectsOfType<SpawnPoint>();
		var player = FindObjectOfType<Player>();

		if (player != null)
		{
			// Spawn warping:
			LastSpawn = string.Empty;
			foreach (var spawn in spawns)
			{
				if (spawn.SpawnPointID == _spawnID)
				{
					if (spawn.newLayer != -1)
					{
						player.ChangeDepthLayer(spawn.newLayer, spawn.newSortingOrder);
					}
					player.transform.position = spawn.transform.position;
					LastSpawn = _spawnID;
					break;
				}
			}
			
			// Snap Camera to grid:
			var cam = FindAnyObjectByType<CameraContoller>();
			cam.SnapToGrid(player.transform.position);
		}

		// Destroy permanent objects:
		Globals.Instance.DestroyDestroyedObjects();
	}

	IEnumerator EnterFadeWait()
	{
		sceneFadeAnimator.Play("CrossfadeEnter");
		IsFading = true;

		yield return new WaitForSeconds(fadeOutTimeInSeconds);
		IsFading = false;
		OnSceneFadeInEnd?.Invoke();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnSceneStart?.Invoke();
		StartCoroutine(EnterFadeWait());
	}
}