using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IPlayerInputHandler : IEventSystemHandler {
	void MoveDown ();
	void MoveLeft ();
	void MoveRight ();
	void RotateLeft ();
	void RotateRight ();
}