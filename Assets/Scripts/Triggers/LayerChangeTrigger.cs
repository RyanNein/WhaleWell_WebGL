using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChangeTrigger : MonoBehaviour
{
	public delegate void SortingEvents();
	public static event SortingEvents OnSortingChange;

	[SerializeField] int newLayer;
	[SerializeField] int newSortingOrder;

	Player player;

	private void Start()
	{
		player = FindObjectOfType<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			player.ChangeDepthLayer(newLayer, newSortingOrder);
			OnSortingChange?.Invoke();
		}
	}

}