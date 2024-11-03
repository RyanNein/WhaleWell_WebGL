using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuackMessage : MonoBehaviour
{
    [SerializeField] float maxTime = 0.25f;
	
	private void Start()
	{
		Destroy(gameObject, maxTime);
	}
}
