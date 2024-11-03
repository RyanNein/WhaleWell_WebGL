using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour, IInteractable
{
	// Scriptable Object:
	[SerializeField] SoItem soItem;
	public SoItem SoItem => soItem;

	SpriteRenderer mySpriteRenderer;

	private void Awake()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		mySpriteRenderer.sprite = soItem.ItemSprite;
	}

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			DialogueSystem.Instance.StartDialogue(SoItem.Story, gameObject);
		}
	}
}