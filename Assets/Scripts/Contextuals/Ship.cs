using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	[SerializeField] GameObject brokenShip;
	[SerializeField] GameObject fixedShip;

	public void SwitchToFixedShip()
	{
		brokenShip.SetActive(false);
		fixedShip.SetActive(true);
	}
}
