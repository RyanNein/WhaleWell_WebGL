using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : NeinUtility.PersistentSingleton<ResolutionManager>
{
	Resolution[] resolutions;
	List<Resolution> filteredResolutions = new List<Resolution>();

	double currentRefreshRate;

	int currentIndex;

	public bool IsFullscreen
	{
		get
		{
			return Screen.fullScreen;
		}
	}

	public List<string> ResolutionsChoices
	{
		get
		{
			GetResolutions();

			List<string> resolutionChoices = new List<string>();
			for (int i = 0; i < filteredResolutions.Count; i++)
			{
				string resolutionChoice = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
				resolutionChoices.Add(resolutionChoice);
			}

			return resolutionChoices;
		}
	}

	private void Start()
	{
		GetResolutions();
		SetResolution(filteredResolutions.Count - 1);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			SetFullscreen(!IsFullscreen);
		}
	}

	public void SetFullscreen(bool _toFull)
	{
		Screen.fullScreen = _toFull;
	}

	void GetResolutions()
	{
		resolutions = Screen.resolutions;
		currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

		filteredResolutions = new List<Resolution>();

		float targetAspectRatio = 4f / 3f;

		for (int i = 0; i < resolutions.Length; i++)
		{
			var aspectRatio = (float)resolutions[i].width / (float)resolutions[i].height;

			if (resolutions[i].refreshRateRatio.value == currentRefreshRate && Mathf.Abs(aspectRatio - targetAspectRatio) < 0.01f)
			{
				filteredResolutions.Add(resolutions[i]);
			}
		}
	}

	public void SetResolution(int _index)
	{
		var resolution = filteredResolutions[_index];
		Screen.SetResolution(resolution.width, resolution.height, IsFullscreen);
	}

	public int GetResolutionIndex()
	{
		for (int i = 0; i < filteredResolutions.Count; i++)
		{
			if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
			{
				currentIndex = i;
			}
		}

		return currentIndex;
	}
}
