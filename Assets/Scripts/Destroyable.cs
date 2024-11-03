using UnityEngine;

public class Destroyable : MonoBehaviour
{
	public string ID;
	public bool AlreadyDestroyed = false;

	public void PermanentDestroy()
	{
		if (!AlreadyDestroyed)
		{
			Globals.Instance.AddDestroyable(ID);
		}
		Destroy(gameObject);
	}
}
