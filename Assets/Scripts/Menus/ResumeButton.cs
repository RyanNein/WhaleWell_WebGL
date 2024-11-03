using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResumeButton : MonoBehaviour, IMenuButton
{
	TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void DoClick()
	{
		GameManager.Instance.DestroyPauseMenu(true);
	}

	public void Highlight(bool _on)
	{

		var alpha = _on ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
