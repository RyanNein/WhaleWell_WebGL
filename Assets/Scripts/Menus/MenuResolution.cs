using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuResolution : MonoBehaviour, IMenuButton
{
	TextMeshProUGUI text;

	List<string> resolutions = new List<string>();
	int resolutionIndex;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		resolutions = ResolutionManager.Instance.ResolutionsChoices;
		resolutionIndex = ResolutionManager.Instance.GetResolutionIndex();
		text.text = resolutions[resolutionIndex];
	}

	public void DoClick()
	{
		resolutionIndex++;
		if (resolutionIndex >= resolutions.Count) resolutionIndex = 0;
		text.text = resolutions[resolutionIndex];
		ResolutionManager.Instance.SetResolution(resolutionIndex);
	}

	public void Highlight(bool _on)
	{
		var alpha = _on == true ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
