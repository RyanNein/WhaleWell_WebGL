using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuToggler : MonoBehaviour, IMenuButton
{
	[SerializeField] GameObject sourcePanel;
	[SerializeField] GameObject newPanel;

	[SerializeField] MainMenu mainMenu;

	TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void DoClick()
	{
		sourcePanel.SetActive(false);
		newPanel.SetActive(true);
		mainMenu.NewMenu(newPanel);
	}

	public void Highlight(bool _on)
	{
		var alpha = _on == true ? 1f : 0.5f;
		text.color = new Color(1f, 1f, 1f, alpha);
	}
}
