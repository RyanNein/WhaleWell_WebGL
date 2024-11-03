using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : MonoBehaviour, IInteractable
{
	[SerializeField] GameObject bloodObject;

	[SerializeField] SoStory story;
	[SerializeField] SoItem item;
	[SerializeField] int requiredNumberOfItem = 1;
	[SerializeField] SoStory hasItemStory;
	[SerializeField] SoStory hasFallenStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (item != null && InventoryManager.Instance.NumberOfItem(item) >= requiredNumberOfItem)
			{
				DialogueSystem.Instance.StartDialogue(hasItemStory, gameObject);
			}
			else if (Globals.Instance.HasFallen)
			{
				DialogueSystem.Instance.StartDialogue(hasFallenStory, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}

	public void ToggleBlood(bool _toOn)
	{
		bloodObject.SetActive(_toOn);
	}
}
