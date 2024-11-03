using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuFullscreen : MonoBehaviour, IMenuButton
{
	TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void DoClick()
	{
		ResolutionManager.Instance.SetFullscreen(!Screen.fullScreen);
	}

	public void Highlight(bool _on)
	{
		var alpha = _on == true ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
