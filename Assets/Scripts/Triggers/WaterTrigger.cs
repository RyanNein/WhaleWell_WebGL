using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class WaterTrigger : MonoBehaviour
{
	[SerializeField] bool isEnterWaterOnEnter;
	[SerializeField] bool isExitWaterOnEnter;
	[SerializeField] bool isExitWaterOnExit;

	[SerializeField] AudioClip waterClip;

	Player player;

	private void Awake()
	{
		GetComponent<Collider2D>().isTrigger = true;
	}

	private void Start()
	{
		player = FindObjectOfType<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
			return;

		if (isEnterWaterOnEnter)
		{
			EnterWater();
		}
		else if (isExitWaterOnEnter)
		{
			ExitWater();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
			return;

		if (isExitWaterOnExit)
		{
			ExitWater();
		}
	}

	private void ExitWater()
	{
		if (player.IsOnWater)
		{
			if (waterClip != null)
			{
				AudioManager.Instance.PlaySFXOneShot(waterClip);
			}
		}
		player.IsOnWater = false;
	}

	private void EnterWater()
	{
		if (!player.IsOnWater)
		{
			if (waterClip != null)
			{
				AudioManager.Instance.PlaySFXOneShot(waterClip);
			}
		}
		player.IsOnWater = true;
	}
}
