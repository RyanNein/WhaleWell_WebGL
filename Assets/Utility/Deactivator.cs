using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeinUtility
{
	public class Deactivator : MonoBehaviour
	{
		private void OnEnable()
		{
			gameObject.SetActive(false);
		}
	}
}