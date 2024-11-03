using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveAndQuit : MonoBehaviour, IMenuButton
{
	TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void DoClick()
	{
		GameManager.Instance.SaveGame();
		GameManager.Instance.DestroyPauseMenu(false);
		SceneLoader.Instance.LoadScene("MainMenu");
	}

	public void Highlight(bool _on)
	{

		var alpha = _on ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
