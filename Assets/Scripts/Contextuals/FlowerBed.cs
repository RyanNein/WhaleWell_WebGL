using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBed : MonoBehaviour
{
	[SerializeField] GameObject flowers;

	public void ActivateFlowers()
	{
		flowers.SetActive(true);
	}
}
