using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {
	public bool[, ] Grid;
	public int Width;
	public int Height;

	private int heightWithPadding;

	public void Initialize (int width = 12, int height = 24) {
		Width = width;
		Height = height;
		heightWithPadding = height + 10;

		Grid = new bool[width, heightWithPadding];
	}

	void Awake () {
		EventManager.Instance.AddListener<MoveBlockGroupEvent> (ValidateMoveHandler);
		EventManager.Instance.AddListener<RotateBlockGroupEvent> (ValidateRotationHandler);
	}

	private void ValidateMoveHandler (MoveBlockGroupEvent e) {
		ValidateMove (e.CurrentPosition, e.Direction);
	}

	private void ValidateRotationHandler (RotateBlockGroupEvent e) {
		ValidateRotation (e.CurrentPosition, e.Direction);
	}

	public void ValidateMove (Transform currentPosition, MoveDirection direction) {
		bool[, ] initalGridState = Grid.Clone () as bool[, ];

		FreeGridForAllBlocksInGroup (currentPosition);

		bool isValid = AssertValidGroupMove (currentPosition, DirectionToVector.For (direction));

		if (isValid) {
			EventManager.Instance.QueueEvent (new MoveValidEvent (currentPosition, direction));
		} else {
			Grid = initalGridState;
			OccupyGridForAllBlocksInGroup (currentPosition);
			EventManager.Instance.QueueEvent (new MoveInvalidEvent (currentPosition, direction));
		}
	}

	public void ValidateRotation (Transform currentPosition, RotateDirection direction) {
		bool[, ] initalGridState = Grid.Clone () as bool[, ];

		FreeGridForAllBlocksInGroup (currentPosition);

		bool isValid = AssertValidGroupRotation (currentPosition, direction);

		if (isValid) {
			EventManager.Instance.QueueEvent (new RotateValidEvent (currentPosition, direction));
		} else {
			Grid = initalGridState;
			OccupyGridForAllBlocksInGroup (currentPosition);
			EventManager.Instance.QueueEvent (new RotateInvalidEvent (currentPosition, direction));
		}

	}

	public bool AssertValidGroupMove (Transform group, Vector3 to) {
		// Get all child transforms
		IEnumerable<Transform> transforms = group.Cast<Transform> ();

		return transforms.Select (t => AssertValidMove (t.position, to)).All (valid => valid == true);
	}

	public bool AssertValidGroupRotation (Transform group, RotateDirection direction) {
		// start by rotating the parent
		group.RotateInDirection (direction);

		// Get all child transforms
		IEnumerable<Transform> transforms = group.Cast<Transform> ();

		// Validate
		bool isValid;
		try {
			isValid = transforms.Select (t => AssertValidTransform (t)).All (valid => valid == true);
		} catch {
			isValid = false;
		}

		// reset block group
		RotateDirection opposite = direction == RotateDirection.Left ? RotateDirection.Right : RotateDirection.Left;
		group.RotateInDirection (opposite);

		return isValid;
	}

	public bool AssertValidMove (Vector3 from, Vector3 to) {
		int x = (int) (from + to).Rounded ().x;
		int y = (int) (from + to).Rounded ().y;
		return AssertValid (x, y);
	}

	public bool AssertValidTransform (Transform block) {
		int x = (int) block.position.Rounded ().x;
		int y = (int) block.position.Rounded ().y;
		return AssertValid (x, y);
	}

	public bool AssertValid (int x, int y) {
		return
		x >= 0 &&
			y >= 0 &&
			x < Grid.GetLength (0) &&
			y < Grid.GetLength (1) &&
			Grid[x, y] == false;
	}

	// void SpawnInGrid (int x, int y) {
	// 	var cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
	// 	cube.transform.position = new Vector3 (x, y, 0);
	// }

	public void Move (Transform group) {
		OccupyGridForAllBlocksInGroup (group);
	}

	private void OccupyGridForAllBlocksInGroup (Transform group) {
		foreach (Transform t in group) {
			int x = (int) t.position.Rounded ().x;
			int y = (int) t.position.Rounded ().y;
			Debug.LogFormat ("Setting true: {0}, {1}", x, y);
			Grid.SetValue (true, x, y);
		}
	}

	private void FreeGridForAllBlocksInGroup (Transform group) {
		foreach (Transform t in group) {
			int x = (int) t.position.Rounded ().x;
			int y = (int) t.position.Rounded ().y;
			Debug.LogFormat ("Setting false: {0}, {1}", x, y);
			Grid.SetValue (false, x, y);
		}
	}

	// Print the grid as a series of 0s and 1s for debugging.
	public void Inspect () {
		var output = "";
		for (int row = Height; row >= 0; row--) {
			output += "\n";
			for (int col = 0; col < Width; col++) {
				var bit = Grid[col, row] ? "1" : "0";
				output += bit;
			}
		}

		DebugCanvas canvas = GameObject.FindObjectOfType (typeof (DebugCanvas)) as DebugCanvas;
		canvas.SetText (output);
	}
}