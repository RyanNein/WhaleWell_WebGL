using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTriggers
{
	public class WhaleNoiseTrigger : MonoBehaviour
	{
		[SerializeField] AudioClip WhaleClip;
		Player player;

		private void Start()
		{
			SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
			mySpriteRenderer.enabled = false;
			player = FindObjectOfType<Player>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.GetComponent<Player>() != null)
			{
				StartCoroutine(noiseScene());
			}
		}

		IEnumerator noiseScene()
		{
			AudioManager.Instance.StopMusic();
			AudioManager.Instance.PlaySFXOneShot(WhaleClip);
			var prevState = player.State;
			player.StateChange(Player.States.None);

			yield return new WaitForSeconds(WhaleClip.length);

			AudioManager.Instance.RestartMusic();
			player.StateChange(prevState);
			// Destroy(gameObject);
			GetComponent<Destroyable>().PermanentDestroy();
		}

	}
}