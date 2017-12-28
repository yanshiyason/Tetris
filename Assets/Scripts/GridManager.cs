using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {
	public int Width;
	public int Height;

	public bool[, ] Grid;

	private static GridManager _instance;

	private int heightWithPadding;

	private Queue<GameEvent> MoveEventsQueue;

	public static GridManager Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof (GridManager)) as GridManager;
			}
			return _instance;
		}
	}

	public void Initialize (int width = 12, int height = 24) {
		Width = width;
		Height = height;
		heightWithPadding = height + 10;

		Grid = new bool[width, heightWithPadding];

		MoveEventsQueue = new Queue<GameEvent> ();

		EventManager.Instance.AddListener<MoveTetrominoEvent> (MoveTetrominoEventHandler);
		EventManager.Instance.AddListener<RotateTetrominoEvent> (RotateTetrominoEventHandler);
	}

	void Update () {
		ProcessQueueItems ();
	}

	private void MoveTetrominoEventHandler (MoveTetrominoEvent e) {
		MoveEventsQueue.Enqueue (e);
	}

	private void RotateTetrominoEventHandler (RotateTetrominoEvent e) {
		MoveEventsQueue.Enqueue (e);
	}

	private void ProcessQueueItems () {
		if (MoveEventsQueue.Count == 0) {
			return;
		}

		GameEvent e = MoveEventsQueue.Dequeue ();

		switch (e.GetType ().Name) {
			case "MoveTetrominoEvent":
				ValidateEvent ((MoveTetrominoEvent) e);
				break;
			case "RotateTetrominoEvent":
				ValidateEvent ((RotateTetrominoEvent) e);
				break;
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

	// TODO: move this to a separate class.
	// **************
	// Validate moves
	// **************

	public void ValidateEvent (MoveTetrominoEvent e) {
		ValidateMove (e.CurrentPosition, e.Direction);
	}

	public void ValidateEvent (RotateTetrominoEvent e) {
		ValidateRotation (e.CurrentPosition, e.Direction);
	}

	public void ValidateMove (Transform currentPosition, MoveDirection direction) {
		bool[, ] initalGridState = Grid.Clone () as bool[, ];

		FreeGridForAllBlocksInTetromino (currentPosition);

		bool isValid = AssertValidTetrominoMove (currentPosition, DirectionToVector.For (direction));

		if (isValid) {
			EventManager.Instance.QueueEvent (new MoveValidEvent (currentPosition, direction));
		} else {
			Grid = initalGridState;
			OccupyGridForAllBlocksInTetromino (currentPosition);
			EventManager.Instance.QueueEvent (new MoveInvalidEvent (currentPosition, direction));
		}
	}

	public void ValidateRotation (Transform currentPosition, RotateDirection direction) {
		bool[, ] initalGridState = Grid.Clone () as bool[, ];

		FreeGridForAllBlocksInTetromino (currentPosition);

		bool isValid = AssertValidTetrominoRotation (currentPosition, direction);

		if (isValid) {
			EventManager.Instance.QueueEvent (new RotateValidEvent (currentPosition, direction));
		} else {
			Grid = initalGridState;
			OccupyGridForAllBlocksInTetromino (currentPosition);
			EventManager.Instance.QueueEvent (new RotateInvalidEvent (currentPosition, direction));
		}

	}

	public bool AssertValidTetrominoMove (Transform tetromino, Vector3 to) {
		// Get all child transforms
		IEnumerable<Transform> transforms = tetromino.Cast<Transform> ();

		return transforms.Select (t => AssertValidMove (t.position, to)).All (valid => valid == true);
	}

	public bool AssertValidTetrominoRotation (Transform tetromino, RotateDirection direction) {
		// start by rotating the parent
		tetromino.RotateInDirection (direction);

		// Get all child transforms
		IEnumerable<Transform> transforms = tetromino.Cast<Transform> ();

		// Validate
		bool isValid;
		try {
			isValid = transforms.Select (t => AssertValidPosition (t.position)).All (valid => valid == true);
		} catch {
			isValid = false;
		}

		// reset block tetromino
		RotateDirection opposite = direction == RotateDirection.Left ? RotateDirection.Right : RotateDirection.Left;
		tetromino.RotateInDirection (opposite);

		return isValid;
	}

	public bool AssertValidMove (Vector3 from, Vector3 to) {
		int x = (int) (from + to).Rounded ().x;
		int y = (int) (from + to).Rounded ().y;
		return AssertValid (x, y);
	}

	public bool AssertValidPosition (Vector3 position) {
		int x = (int) position.Rounded ().x;
		int y = (int) position.Rounded ().y;
		return AssertValid (x, y);
	}

	public bool AssertValid (int x, int y) {
		return
		x >= 0 &&
			y >= 0 &&
			x < Grid.GetLength (0) &&
			Grid[x, y] == false;
	}

	public void Move (Transform tetromino) {
		OccupyGridForAllBlocksInTetromino (tetromino);
	}

	private void OccupyGridForAllBlocksInTetromino (Transform tetromino) {
		foreach (Transform t in tetromino) {
			int x = (int) t.position.Rounded ().x;
			int y = (int) t.position.Rounded ().y;
			Debug.LogFormat ("Setting true: {0}, {1}", x, y);
			Grid.SetValue (true, x, y);
		}
	}

	private void FreeGridForAllBlocksInTetromino (Transform tetromino) {
		foreach (Transform t in tetromino) {
			int x = (int) t.position.Rounded ().x;
			int y = (int) t.position.Rounded ().y;
			Debug.LogFormat ("Setting false: {0}, {1}", x, y);
			Grid.SetValue (false, x, y);
		}
	}
}