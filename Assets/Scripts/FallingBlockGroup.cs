using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FallingBlockGroup : MonoBehaviour {
	Rigidbody rigidbody;

	// Use this for initialization
	void Awake () {
		rigidbody = gameObject.AddComponent<Rigidbody> ();
		rigidbody.useGravity = false;

		EventManager.Instance.AddListener<MoveValidEvent> (MoveValid);
		EventManager.Instance.AddListener<MoveInvalidEvent> (MoveInvalid);

		EventManager.Instance.AddListener<RotateValidEvent> (RotateValid);
		EventManager.Instance.AddListener<RotateInvalidEvent> (RotateInvalid);
	}

	void OnDestroy () {
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
	}

	public void RotateValid (RotateValidEvent e) {
		Debug.Log ("Rotation is valid");
		transform.RotateInDirection (e.Direction);

		TetrisManager.GridManager.Move (transform);

		TetrisManager.GridManager.Inspect ();
	}

	public void RotateInvalid (RotateInvalidEvent e) {
		Debug.Log ("Rotation is invalid");
	}

}