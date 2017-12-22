using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

class PlayerInputListener : MonoBehaviour {
	void Update () {
		ListenToPlayerInput ();
	}

	void ListenToPlayerInput () {
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.RightArrow)) {
			EventManager.TriggerEvent ("RotateRight");
		} else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.LeftArrow)) {
			EventManager.TriggerEvent ("RotateLeft");
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			EventManager.TriggerEvent ("MoveLeft");
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			EventManager.TriggerEvent ("MoveRight");
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			EventManager.TriggerEvent ("MoveDown");
		}
	}
}