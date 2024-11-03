using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlocker : MonoBehaviour
{
	[SerializeField] string ahievementID;

	private void Start()
	{
		//SteamManager.Instance.UnlockAchievement(ahievementID);
	}
}
