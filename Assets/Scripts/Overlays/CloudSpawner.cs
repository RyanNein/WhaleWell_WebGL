using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudSystem
{
	public class CloudSpawner : MonoBehaviour
	{
		[SerializeField] GameObject cloudPrefab;
		[SerializeField] bool movesRight;
		private Vector2 direction;

		[SerializeField] float rangeMax;
		[SerializeField] float rangeMin;

		private float timerMax;
		private float currentTime = 0f;

		private void Start()
		{
			ResetRange();
			direction = movesRight == true ? Vector2.right : Vector2.left;
		}


		private void Update()
		{
			currentTime -= Time.deltaTime;
			if (currentTime <= 0)
			{
				Cloud.CreateCloud(cloudPrefab, gameObject, direction);
				ResetRange();
				currentTime = timerMax;
			}
		}

		private void ResetRange()
		{
			timerMax = Random.Range(rangeMin, rangeMax);
		}
	}
}