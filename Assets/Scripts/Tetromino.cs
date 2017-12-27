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

	// Use this for initialization
	void Awake () {
		State = TetrominoState.falling;

		// Setup Input Listener
		PlayerInputListener = gameObject.AddComponent<PlayerInputListener> ();

		StartListeningForMovementEvents ();
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
		Debug.Log ("Move is valid");
		transform.Move (e.Direction);

		TetrisManager.GridManager.Move (transform);
		TetrisManager.GridManager.Inspect ();
	}

	public void MoveInvalid (MoveInvalidEvent e) {
		Debug.Log ("Move is invalid");

		if (e.Direction == MoveDirection.Down) {
			State = TetrominoState.landed;
			Destroy (PlayerInputListener);
			StopListeningForMovementEvents ();
			Debug.Log ("Player event listener on tetromino destroyed");

			EventManager.Instance.QueueEvent (new SpawnTetrominoEvent ());
		}
	}

	public void RotateValid (RotateValidEvent e) {
		Debug.Log ("Rotation is valid");

		Rotate (e.Direction);

		TetrisManager.GridManager.Move (transform);

		TetrisManager.GridManager.Inspect ();
	}

	public void RotateInvalid (RotateInvalidEvent e) {
		Debug.Log ("Rotation is invalid");
	}

	void Rotate (RotateDirection direction) {
		switch (ValidRotations) {
			case ValidRotations.full:
				Debug.Log ("valid full");
				transform.RotateInDirection (direction);
				break;
			case ValidRotations.none:
				Debug.Log ("valid none");
				break;
			case ValidRotations.half:
				Debug.Log ("valid half");
				ToggleRotate ();
				break;
		}

	}

	void ToggleRotate () {
		Debug.LogFormat ("transform.localEulerAngles.z: {0}", transform.localEulerAngles.z);

		RotateDirection direction = transform.localEulerAngles.z == 90 ? RotateDirection.Left : RotateDirection.Right;
		transform.RotateInDirection (direction);
		transform.Rounded ();
	}

}