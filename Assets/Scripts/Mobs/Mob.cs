using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Mob : MonoBehaviour, IInteractable
{
	[SerializeField] SoStory story;
	[SerializeField] SoItem item;
	[SerializeField] int requiredNumberOfItem = 1;
	[SerializeField] SoStory hasItemStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (item != null && InventoryManager.Instance.NumberOfItem(item) >= requiredNumberOfItem)
			{
				DialogueSystem.Instance.StartDialogue(hasItemStory, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}
}
