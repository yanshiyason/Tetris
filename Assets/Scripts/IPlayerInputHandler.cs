using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Tetris.Extensions;


public interface IPlayerInputHandler : IEventSystemHandler
{
	void MoveDown();
	void MoveLeft();
	void MoveRight();
	void RotateLeft();
	void RotateRight();
}