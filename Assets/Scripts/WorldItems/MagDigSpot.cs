using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagDigSpot : MonoBehaviour, IInteractable
{
	[SerializeField] SoStory story;
	[SerializeField] SoItem shovel;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (InventoryManager.Instance.HasItem(shovel))
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}
}
