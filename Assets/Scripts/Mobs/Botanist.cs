using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Botanist : MonoBehaviour, IInteractable
{
	[SerializeField] SoStory story;
	[SerializeField] SoItem flowersSo;
	[SerializeField] int requiredNumberOfFlowers = 3;
	[SerializeField] SoStory hasFlowersStory;
	[SerializeField] SoStory needsFuelStory;
	[SerializeField] SoItem fuelSo;
	[SerializeField] SoStory hasFuelStory;
	[SerializeField] SoStory finishStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (InventoryManager.Instance.NumberOfItem(flowersSo) >= requiredNumberOfFlowers)
			{
				DialogueSystem.Instance.StartDialogue(hasFlowersStory, gameObject);
			}
			else if (InventoryManager.Instance.HasItem(fuelSo) && Globals.Instance.HasGivenFlowers)
			{
				DialogueSystem.Instance.StartDialogue(hasFuelStory, gameObject);
			}
			if (Globals.Instance.HasGivenFlowers && !Globals.Instance.HasGivenFuel)
			{
				DialogueSystem.Instance.StartDialogue(needsFuelStory, gameObject);
			}
			else if (Globals.Instance.HasGivenFlowers && Globals.Instance.HasGivenFuel)
			{
				DialogueSystem.Instance.StartDialogue(finishStory, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}
}
