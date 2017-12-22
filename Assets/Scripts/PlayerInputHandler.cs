﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tetris.Extensions;
using System.Linq;

public class PlayerInputHandler : MonoBehaviour, IPlayerInputHandler {
	UnityAction moveDownListener;
	UnityAction moveLeftListener;
	UnityAction moveRightListener;
	UnityAction rotateLeftListener;
	UnityAction rotateRightListener;
	void Awake()
	{
		moveDownListener = new UnityAction(MoveDown);
		moveLeftListener = new UnityAction(MoveLeft);
		moveRightListener = new UnityAction(MoveRight);
		rotateLeftListener = new UnityAction(RotateLeft);
		rotateRightListener = new UnityAction(RotateRight);

		RegisterListener("MoveDown", moveDownListener);
		RegisterListener("MoveLeft", moveLeftListener);
		RegisterListener("MoveRight", moveRightListener);
		RegisterListener("RotateLeft", rotateLeftListener);
		RegisterListener("RotateRight", rotateRightListener);
	}

	public void MoveDown()
	{
		Debug.Log("Received MoveDown()");
		// transform

		IEnumerable<Transform> transforms = TetrisManager.fallingBlockGroup.transform.Cast<Transform>();
		foreach (var t in transforms) {
			Debug.Log(t.position);
			Debug.Log(TetrisManager.grid.AssertValidMove(t.position, new Vector3(0, -1, 0)));
		}

		bool isValid =
			transforms
				.Select(t => TetrisManager.grid.AssertValidMove(
					t.position, new Vector3(0, -1, 0))
				)
				.All(v => v == true);

		

		if (isValid) {
			Debug.Log("Was Valid");
			TetrisManager.fallingBlockGroup.transform.MoveDown();
		} else {
			Debug.Log("Was Invalid");
		}
		TetrisManager.fallingBlockGroup.transform.LogTransforms();
	}

	public void MoveLeft()
	{
		Debug.Log("Received MoveLeft()");
		TetrisManager.fallingBlockGroup.transform.MoveLeft();
		TetrisManager.fallingBlockGroup.transform.LogTransforms();
	}

	public void MoveRight()
	{
		Debug.Log("Received MoveRight()");
		TetrisManager.fallingBlockGroup.transform.MoveRight();
		TetrisManager.fallingBlockGroup.transform.LogTransforms();
	}

	public void RotateLeft()
	{
		Debug.Log("Received RotateLeft()");
		TetrisManager.fallingBlockGroup.transform.RotateLeft();
		TetrisManager.fallingBlockGroup.transform.LogTransforms();
	}

	public void RotateRight()
	{
		Debug.Log("Received RotateRight()");
		TetrisManager.fallingBlockGroup.transform.RotateRight();
		TetrisManager.fallingBlockGroup.transform.LogTransforms();
	}

	private void RegisterListener(string eventName, UnityAction listener)
	{
		UnityEvent unityEvent = null;
		PlayerInputListener.eventDictionary.TryGetValue(eventName, out unityEvent);
		if (unityEvent == null)
			return;

		unityEvent.AddListener(listener);
	}

}
