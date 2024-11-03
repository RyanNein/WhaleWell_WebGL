using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{

	[SerializeField] GameObject factorySceneTrigger;

	public void CreateSceneTrigger()
	{
		factorySceneTrigger.SetActive(true);
	}
}
