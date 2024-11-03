using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherParticle : MonoBehaviour
{
	[SerializeField] float speed = 1f;

	private void Update()
	{
		transform.Translate(Vector2.up * speed * Time.deltaTime);
	}
}


