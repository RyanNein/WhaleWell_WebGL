using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudSystem
{
	public class Cloud : MonoBehaviour
	{
		private static List<Cloud> _activeClouds;
		public static List<Cloud> ActiveClouds
		{
			get
			{
				if (_activeClouds == null)
					_activeClouds = new List<Cloud>();

				return _activeClouds;
			}
		}

		public delegate void CloudEvents(Cloud cloud);
		public static event CloudEvents OnCloudChange;

		public static void CreateCloud(GameObject prefab, GameObject parentObject, Vector2 directionVector)
		{
			GameObject inst = Instantiate(prefab, parentObject.transform);
			Cloud instCloud = inst.GetComponent<Cloud>();

			instCloud.direction = directionVector;
			ActiveClouds.Add(instCloud);

			OnCloudChange?.Invoke(instCloud);
		}

		[SerializeField] float speed = 1f;
		[SerializeField] Vector2 direction;

		[SerializeField] SpriteRenderer mySpriteRenderer;
		[SerializeField] Sprite[] cloudSprites;

		private void Start()
		{
			mySpriteRenderer.sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
		}

		private void Update()
		{
			Vector2 deltaPosition = direction * speed * Time.deltaTime;
			transform.Translate(deltaPosition);

			if (transform.position.sqrMagnitude > 2500f)
			{
				ActiveClouds.Remove(this);
				OnCloudChange?.Invoke(this);
				Destroy(gameObject);
			}
		}

	}
}