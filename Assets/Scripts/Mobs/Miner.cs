using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour, IInteractable
{
	Animator animator;

	[SerializeField] SoStory story;
	[SerializeField] SoItem item;
	[SerializeField] int requiredNumberOfItem = 1;
	[SerializeField] SoStory hasItemStory;
	[SerializeField] SoStory finishedStory;

	public void PlayerInteraction()
	{
		if (DialogueSystem.Instance.canStartDialogue)
		{
			if (item != null && InventoryManager.Instance.NumberOfItem(item) >= requiredNumberOfItem)
			{
				DialogueSystem.Instance.StartDialogue(hasItemStory, gameObject);
			}
			else if (Globals.Instance.HasGivenMedkit)
			{
				DialogueSystem.Instance.StartDialogue(finishedStory, gameObject);
			}
			else
			{
				DialogueSystem.Instance.StartDialogue(story, gameObject);
			}
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void SwitchToNoPickaxe()
	{
		animator.SetBool("HasMedkit", true);
	}
}
