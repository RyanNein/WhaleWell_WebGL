using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGlobalTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Globals.Instance.HasFallen = true;
	}
}
