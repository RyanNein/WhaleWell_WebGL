using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] AudioClip myClip;

	private void Start()
	{
		AudioManager.Instance.PlayMusic(myClip);
	}
}
