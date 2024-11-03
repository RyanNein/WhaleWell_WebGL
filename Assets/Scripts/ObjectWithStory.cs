using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWithStory : MonoBehaviour, IInteractable
{
	[SerializeField] SoStory objectStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			DialogueSystem.Instance.StartDialogue(objectStory);
		}
	}
}
