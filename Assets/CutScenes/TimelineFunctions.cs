using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineFunctions : MonoBehaviour
{

	private void Awake()
	{
		var cameraPosition = Camera.main.transform.position;
		cameraPosition.z = 0f;
		transform.position = cameraPosition;
	}
	public void LoadLake()
	{
		SceneLoader.Instance.LoadScene("LakeArea", "Whale");
	}
}
