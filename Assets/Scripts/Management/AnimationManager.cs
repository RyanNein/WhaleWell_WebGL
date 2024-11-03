using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : NeinUtility.PersistentSingleton<AnimationManager>
{

	int animationFrame = 0;
	public int AnimationFrame => animationFrame;

    public float frameDuration = 0.5f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            animationFrame = (animationFrame + 1) % 2;
        }
    }
}
