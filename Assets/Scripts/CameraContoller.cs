using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraContoller : MonoBehaviour
{
	const int SCREEN_HEIGHT = 12;
	const int SCREEN_WIDTH = 16;

	Player player;

	private void Start()
	{
		player = FindObjectOfType<Player>();
	}

	public delegate void CameraMovementEvents();
	public static event CameraMovementEvents
		onCameraStartMovement,
		onCameraStopMovement;


	[SerializeField] private int speed = 6;
	[SerializeField] private Transform targetTransform;

	private bool targetFound = true;

	private void LateUpdate()
	{
		if (targetFound)
			return;

		transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
		if (Vector3.Distance(transform.position, targetTransform.position) < 0.125)
		{
			// snap to target
			transform.position = targetTransform.position;
			targetFound = true;

			player.StateChange(player.PreviousState);

			onCameraStopMovement?.Invoke();
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!targetFound)
			return;

		if (collision.CompareTag("Player"))
		{
			var playerDirection = player.LookDirection;
			TransitionScreens(playerDirection);
		}
	}

	private void TransitionScreens(Vector2 direction)
	{
		targetFound = false;

		player.StateChange(Player.States.None);

		Vector3 playerDirection = new Vector3(direction.x, direction.y, 0);
		Vector3 targetPosition = targetTransform.position;

		targetPosition += playerDirection.x == 0 ? SCREEN_HEIGHT * playerDirection : SCREEN_WIDTH * playerDirection;
		targetPosition = Vector3Int.RoundToInt(targetPosition);
		targetTransform.position = targetPosition;

		onCameraStartMovement?.Invoke();
	}

	public void SnapToGrid(Vector2 _position)
	{
		Vector3 snappedPosition = new Vector3(
			Mathf.Round(_position.x / SCREEN_WIDTH) * SCREEN_WIDTH,
			Mathf.Round(_position.y / SCREEN_HEIGHT) * SCREEN_HEIGHT,
			transform.position.z
		);

		transform.position = snappedPosition;
		targetTransform.position = snappedPosition;
	}
}