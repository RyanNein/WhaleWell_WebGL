using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTile : MonoBehaviour
{
	[SerializeField] Sprite[] sprites;

	SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		spriteRenderer.sprite = sprites[AnimationManager.Instance.AnimationFrame];
	}
}
