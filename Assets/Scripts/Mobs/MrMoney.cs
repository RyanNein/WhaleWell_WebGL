using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MrMoney : MonoBehaviour, IInteractable
{
	[SerializeField] SoStory story;
	[SerializeField] SoItem item;
	[SerializeField] int requiredNumberOfItem = 1;
	[SerializeField] SoStory hasItemStory;
	[SerializeField] SoStory finishedStory;
	[SerializeField] SoStory reactionStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (item != null && InventoryManager.Instance.NumberOfItem(item) >= requiredNumberOfItem)
			{
				DialogueSystem.Instance.StartDialogue(hasItemStory, gameObject);
			}
			else if (Globals.Instance.HasGivenOil && Globals.Instance.HasGivenMedkit)
			{
				DialogueSystem.Instance.StartDialogue(reactionStory, gameObject);
			}
			else if (Globals.Instance.HasGivenOil)
			{
				DialogueSystem.Instance.StartDialogue(finishedStory, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}
}
