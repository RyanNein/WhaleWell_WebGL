using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class SoStory : ScriptableObject
{
	public Page[] Pages;

	private void OnValidate()
	{
		for (int i = 0; i < Pages.Length; i++)
		{
			Pages[i].PageIndex = i;
		}
	}
}

[System.Serializable]
public class Page
{
	public int PageIndex;
	public bool IsLastPage;
	public string methodName;
	public Sprite Portrait;

	[TextArea]
	public string text;

	public PageOption[] options;
}

[System.Serializable]
public class PageOption
{
	public string text;
	public int nextPage;
}