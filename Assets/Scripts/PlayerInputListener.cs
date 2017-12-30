using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

class PlayerInputListener : MonoBehaviour {
	Tetromino tetromino;

	static float keyPressDelay = 0.05f;
	static float lastKeyPressAt;

	void Awake () {
		tetromino = GetComponentInParent<Tetromino> ();
	}

	void Start () {
		StartCoroutine (MoveBlockDownEachInterval ());
	}

	void Update () {
		GetPlayerInput ();
	}

	void GetPlayerInput () {
		if (Time.time - lastKeyPressAt < keyPressDelay) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			MoveLeft ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			MoveRight ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			MoveDown ();
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			MoveDown ();
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			MoveLeft ();
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			MoveRight ();
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			RotateLeft ();
		} else {
			return;
		}

		lastKeyPressAt = Time.time;
	}

	void MoveDown () {
		EventManager.Instance.TriggerEvent (new MoveTetrominoEvent (tetromino.transform, MoveDirection.Down));
	}

	void MoveLeft () {
		EventManager.Instance.TriggerEvent (new MoveTetrominoEvent (tetromino.transform, MoveDirection.Left));
	}
	void MoveRight () {
		EventManager.Instance.TriggerEvent (new MoveTetrominoEvent (tetromino.transform, MoveDirection.Right));
	}

	void RotateLeft () {
		EventManager.Instance.TriggerEvent (new RotateTetrominoEvent (tetromino.transform, RotateDirection.Left));
	}
	void RotateRight () {
		EventManager.Instance.TriggerEvent (new RotateTetrominoEvent (tetromino.transform, RotateDirection.Right));
	}

	IEnumerator MoveBlockDownEachInterval () {
		while (true) {
			yield return new WaitForSeconds (1);
			MoveDown ();
		}
	}
}