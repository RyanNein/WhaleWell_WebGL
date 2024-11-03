using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeDisplay : MonoBehaviour
{
	[SerializeField] GameObject pickaxe;

	public void ActivatePickaxe()
	{
		pickaxe.SetActive(true);
	}
}
