using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisherScript : Mob
{
	[SerializeField] Transform[] targetPoints;
	private int targetIndex = 0;
	[SerializeField] float speed = 1f;

	bool moving = true;

	private void Update()
	{
		if (moving)
		{
			Vector2 moveVector = Vector2.MoveTowards(transform.position, targetPoints[targetIndex].position, speed * Time.deltaTime);
			transform.position = moveVector;

			if (Vector2.Distance(transform.position, targetPoints[targetIndex].position) < .125)
			{
				transform.position = targetPoints[targetIndex].position;
				targetIndex++;
			
				if(targetIndex >= targetPoints.Length)
					targetIndex = 0;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			moving = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			moving = true;
		}
	}
}
