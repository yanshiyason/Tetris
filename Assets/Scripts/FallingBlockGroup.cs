using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FallingBlockGroup : MonoBehaviour, IPlayerInputHandler {
	UnityAction moveDownListener;
	UnityAction moveLeftListener;
	UnityAction moveRightListener;
	UnityAction rotateLeftListener;
	UnityAction rotateRightListener;

	Rigidbody rigidbody;

	// Use this for initialization
	void Awake () {
		Debug.Log ("Awakening");
		rigidbody = gameObject.AddComponent<Rigidbody> ();
		rigidbody.useGravity = false;

		moveDownListener = new UnityAction (MoveDown);
		moveLeftListener = new UnityAction (MoveLeft);
		moveRightListener = new UnityAction (MoveRight);
		rotateLeftListener = new UnityAction (RotateLeft);
		rotateRightListener = new UnityAction (RotateRight);

		EventManager.RegisterListener ("MoveDownValid", moveDownListener);
		EventManager.RegisterListener ("MoveLeftValid", moveLeftListener);
		EventManager.RegisterListener ("MoveRightValid", moveRightListener);
		EventManager.RegisterListener ("RotateLeftValid", rotateLeftListener);
		EventManager.RegisterListener ("RotateRightValid", rotateRightListener);
	}

	void OnDestroy () {
		EventManager.DestroyListener ("MoveDownValid", moveDownListener);
		EventManager.DestroyListener ("MoveLeftValid", moveLeftListener);
		EventManager.DestroyListener ("MoveRightValid", moveRightListener);
		EventManager.DestroyListener ("RotateLeftValid", rotateLeftListener);
		EventManager.DestroyListener ("RotateRightValid", rotateRightListener);
	}

	public void MoveDown () {
		Debug.Log ("Received MoveDown()");
		// transform

		IEnumerable<Transform> transforms = transform.Cast<Transform> ();
		foreach (var t in transforms) {
			Debug.Log (t.position);
			Debug.Log (TetrisManager.gridManager.AssertValidMove (t.position, new Vector3 (0, -1, 0)));
		}

		bool isValid =
			transforms
			.Select (t => TetrisManager.gridManager.AssertValidMove (
				t.position, new Vector3 (0, -1, 0)))
			.All (v => v == true);

		if (isValid) {
			Debug.Log ("Was Valid");
			transform.MoveDown ();
		} else {
			Debug.Log ("Was Invalid");
		}
		transform.LogTransforms ();
	}

	public void MoveLeft () {
		Debug.Log ("Received MoveLeft()");
		transform.MoveLeft ();
		transform.LogTransforms ();
	}

	public void MoveRight () {
		Debug.Log ("Received MoveRight()");
		transform.MoveRight ();
		transform.LogTransforms ();
	}

	public void RotateLeft () {
		Debug.Log ("Received RotateLeft()");
		transform.RotateLeft ();
		transform.LogTransforms ();
	}

	public void RotateRight () {
		Debug.Log ("Received RotateRight()");
		transform.RotateRight ();
		transform.LogTransforms ();
	}
}