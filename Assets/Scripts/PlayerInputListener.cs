using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

class PlayerInputListener : MonoBehaviour {
	void Start () {

	}

	void Update () {
		ListenToPlayerInput ();
	}

	void ListenToPlayerInput () {
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.RightArrow)) {
			EventManager.Instance.QueueEvent (new RotateBlockGroupEvent (TetrisManager.fallingBlockGroup.transform, RotateDirection.Right));
		} else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.LeftArrow)) {
			EventManager.Instance.QueueEvent (new RotateBlockGroupEvent (TetrisManager.fallingBlockGroup.transform, RotateDirection.Left));
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			EventManager.Instance.QueueEvent (new MoveBlockGroupEvent (TetrisManager.fallingBlockGroup.transform, MoveDirection.Left));
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			EventManager.Instance.QueueEvent (new MoveBlockGroupEvent (TetrisManager.fallingBlockGroup.transform, MoveDirection.Right));
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			EventManager.Instance.QueueEvent (new MoveBlockGroupEvent (TetrisManager.fallingBlockGroup.transform, MoveDirection.Down));
		}
	}
}