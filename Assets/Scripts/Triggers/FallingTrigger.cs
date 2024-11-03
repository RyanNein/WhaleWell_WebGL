using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
	Player player;

	private void Start()
	{
		player = FindObjectOfType<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		player.StateChange(Player.States.Falling);
	}
}