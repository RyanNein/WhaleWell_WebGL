using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReturnToMainMenuButton : MonoBehaviour, IMenuButton
{
	[SerializeField] MainMenu mainMenu;
	[SerializeField] string sceneID;

	TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void DoClick()
	{
		GameManager.Instance.EraseSave();
		SceneLoader.Instance.LoadScene(sceneID);
		mainMenu.buttonIndex = -1;
	}

	public void Highlight(bool _on)
	{

		var alpha = _on ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
