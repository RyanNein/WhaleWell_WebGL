using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkParticleEmitter : MonoBehaviour
{
	[SerializeField] float minWait;
	[SerializeField] float maxWait;

	[SerializeField] ParticleSystem largeinkEmitter;
	[SerializeField] ParticleSystem smallinkEmitter;

	AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		StartCoroutine(EmitAndWait());
	}

	IEnumerator EmitAndWait()
	{
		while (true)
		{
			audioSource.Play();
			largeinkEmitter.Play();
			smallinkEmitter.Play();
			yield return new WaitForSeconds(Random.Range(minWait, maxWait));
		}
	}
}
