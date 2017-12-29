using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;

public enum ValidRotations { full, half, none }

public enum TetrominoState { falling, landed }

public class Tetromino : MonoBehaviour {
	[SerializeField] public ValidRotations ValidRotations;

	public TetrominoState State;
	private PlayerInputListener PlayerInputListener;
	private GridManager GridManager;

	private static Color[] TetrominoColors = { Color.blue, Color.red, Color.green, Color.yellow, Color.magenta };

	// Use this for initialization
	void Awake () {
		SetRandomColor ();
		State = TetrominoState.falling;

		GridManager = FindObjectOfType<GridManager> ();

		// Setup Input Listener
		PlayerInputListener = gameObject.AddComponent<PlayerInputListener> ();

		StartListeningForMovementEvents ();
	}

	void SetRandomColor () {
		var i = Random.Range (0, TetrominoColors.Length);
		var color = TetrominoColors[i];

		var renderers = gameObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in renderers) {
			renderer.material.color = color;
		}
	}

	void StartListeningForMovementEvents () {
		EventManager.Instance.AddListener<MoveValidEvent> (MoveValid);
		EventManager.Instance.AddListener<MoveInvalidEvent> (MoveInvalid);

		EventManager.Instance.AddListener<RotateValidEvent> (RotateValid);
		EventManager.Instance.AddListener<RotateInvalidEvent> (RotateInvalid);
	}

	void StopListeningForMovementEvents () {
		EventManager.Instance.RemoveListener<MoveValidEvent> (MoveValid);
		EventManager.Instance.RemoveListener<MoveInvalidEvent> (MoveInvalid);

		EventManager.Instance.RemoveListener<RotateValidEvent> (RotateValid);
		EventManager.Instance.RemoveListener<RotateInvalidEvent> (RotateInvalid);
	}

	public void MoveValid (MoveValidEvent e) {
		transform.Move (e.Direction);

		GridManager.Instance.Grid.Move (transform);
		GridManager.Instance.Inspect ();
	}

	public void MoveInvalid (MoveInvalidEvent e) {
		if (e.Direction == MoveDirection.Down) {
			State = TetrominoState.landed;
			Destroy (PlayerInputListener);
			StopListeningForMovementEvents ();

			EventManager.Instance.QueueEvent (new TetrominoLandedEvent ());
		}
	}

	public void RotateValid (RotateValidEvent e) {
		Rotate (e.Direction);

		GridManager.Instance.Grid.Move (transform);

		GridManager.Instance.Inspect ();
	}

	public void RotateInvalid (RotateInvalidEvent e) { }

	void Rotate (RotateDirection direction) {
		switch (ValidRotations) {
			case ValidRotations.full:
				transform.RotateInDirection (direction);
				break;
			case ValidRotations.none:
				break;
			case ValidRotations.half:
				ToggleRotate ();
				break;
		}

	}

	void ToggleRotate () {
		RotateDirection direction = transform.localEulerAngles.z == 90 ? RotateDirection.Left : RotateDirection.Right;
		transform.RotateInDirection (direction);
		transform.Rounded ();
	}

}