using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSpawner : MonoBehaviour
{
	[SerializeField] float rangeMax;
	[SerializeField] float rangeMin;

	[SerializeField] GameObject smokePrefab;

	private float timerMax;
	private float currentTime = 0f;
	
	private void Start()
	{
		ResetRange();
	}

	private void Update()
	{
		currentTime -= Time.deltaTime;
		if (currentTime <= 0)
		{
			Instantiate(smokePrefab, transform);
			ResetRange();
			currentTime = timerMax;
		}
	}

	private void ResetRange()
	{
		timerMax = Random.Range(rangeMin, rangeMax);
	}
}
