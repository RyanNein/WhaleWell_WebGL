using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuNewGame : MonoBehaviour, IMenuButton
{
	[SerializeField] MainMenu mainMenu;
	TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void DoClick()
	{
		GameManager.Instance.StartNewGame();
		mainMenu.buttonIndex = -1;
	}

	public void Highlight(bool _on)
	{
		var alpha = _on == true ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
