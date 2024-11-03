using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaincoatMob : MonoBehaviour
{
	Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void SwitchToNoShovel()
	{
		animator.SetBool("HasShovel", false);
	}
}
