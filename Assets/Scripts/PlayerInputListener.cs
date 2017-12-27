using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

class PlayerInputListener : MonoBehaviour {
	Tetromino tetromino;

	void Awake () {
		tetromino = GetComponentInParent<Tetromino> ();
	}

	void Start () {
		StartCoroutine (MoveBlockDownEachInterval ());
	}

	void Update () {
		ListenToPlayerInput ();
	}

	void ListenToPlayerInput () {
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.RightArrow)) {
			RotateLeft ();
		} else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.LeftArrow)) {
			RotateRight ();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			MoveLeft ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			MoveRight ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			MoveDown ();
		}
	}

	void MoveDown () {
		EventManager.Instance.QueueEvent (new MoveTetrominoEvent (tetromino.transform, MoveDirection.Down));
	}

	void MoveLeft () {
		EventManager.Instance.QueueEvent (new MoveTetrominoEvent (tetromino.transform, MoveDirection.Left));
	}
	void MoveRight () {
		EventManager.Instance.QueueEvent (new MoveTetrominoEvent (tetromino.transform, MoveDirection.Right));
	}

	void RotateLeft () {
		EventManager.Instance.QueueEvent (new RotateTetrominoEvent (tetromino.transform, RotateDirection.Right));
	}
	void RotateRight () {
		EventManager.Instance.QueueEvent (new RotateTetrominoEvent (tetromino.transform, RotateDirection.Left));
	}

	IEnumerator MoveBlockDownEachInterval () {
		while (true) {
			yield return new WaitForSeconds (1);
			MoveDown ();
		}
	}
}