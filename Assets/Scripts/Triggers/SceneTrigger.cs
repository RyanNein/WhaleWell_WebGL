using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneLoading
{
	[RequireComponent(typeof(Collider2D))]
    public class SceneTrigger : MonoBehaviour
    {

		[SerializeField] string sceneToLoad;
		[SerializeField] string spawnID;

		private void Awake()
		{
			GetComponent<Collider2D>().isTrigger = true;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (DialogueSystem.Instance.isDialogueActive)
				return;

			var player = collision.GetComponent<Player>();

			if (player != null)
			{
				SceneLoader.Instance.LoadScene(sceneToLoad, spawnID);
				player.StateChange(Player.States.None);
			}
		}
	}
}