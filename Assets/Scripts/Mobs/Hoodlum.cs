using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hoodlum : MonoBehaviour, IInteractable
{
	[SerializeField] SoStory story;
	[SerializeField] SoStory foundTreasureStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (Globals.Instance.HasPickedUpMag)
			{
				DialogueSystem.Instance.StartDialogue(foundTreasureStory, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}
}
