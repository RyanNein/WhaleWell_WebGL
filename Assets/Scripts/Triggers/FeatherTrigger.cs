using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherTrigger : MonoBehaviour
{

	[SerializeField] GameObject featherParticle;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
			return;

		if (!Globals.Instance.HasFallen)
		{
			Instantiate(featherParticle, collision.gameObject.transform.position, Quaternion.identity);
		}
	}
}
