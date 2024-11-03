using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoItem : ScriptableObject
{
	public string ItemName;
	public Sprite ItemSprite;

	public SoStory Story;
}
