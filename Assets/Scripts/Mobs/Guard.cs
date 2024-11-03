using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
	[SerializeField] Vector3 movedPosition;

	bool moving = false;
	[SerializeField] float speed = 1f;

	private void Update()
	{
		if (moving)
		{
			Vector2 moveVector = Vector2.MoveTowards(transform.position, movedPosition, speed * Time.deltaTime);
			transform.position = moveVector;

			if (Vector2.Distance(transform.position, movedPosition) < .125)
			{
				transform.position = movedPosition;
				moving = false;
			}
		}
	}

	public void WarpToRight()
	{
		transform.position = movedPosition;
	}

	public void WalkToRight()
	{
		moving = true;
	}
}
