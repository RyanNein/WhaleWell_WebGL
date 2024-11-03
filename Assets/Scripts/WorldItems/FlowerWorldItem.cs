using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FlowerWorldItem : MonoBehaviour, IInteractable
{
	// Scriptable Object:
	[SerializeField] SoItem soFlower;
	[SerializeField] SoItem soShovel;

	[SerializeField] SoStory noShovelStory;

	SpriteRenderer mySpriteRenderer;

	private void Awake()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		mySpriteRenderer.sprite = soFlower.ItemSprite;
	}
	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (InventoryManager.Instance.HasItem(soShovel))
			{
				DialogueSystem.Instance.StartDialogue(soFlower.Story, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(noShovelStory);
			}
		}
	}
}
