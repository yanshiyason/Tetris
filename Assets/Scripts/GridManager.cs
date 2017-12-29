using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Grid {
	public Transform[, ] Value;
	public int Width;
	public int Height;

	public Transform this [int col, int row] {
		get { return Value[col, row]; }
		set { Value.SetValue (value, col, row); }
	}

	public Grid (int width, int height) {
		Width = width;
		Height = height;
		Value = new Transform[width, height];
	}

	public void Move (Transform tetromino) {
		OccupyGridForTetromino (tetromino);
	}

	public void OccupyGridForTetromino (Transform tetromino) {
		foreach (Transform block in tetromino) {
			OccupyGridForSingleBlock (block);
		}
	}

	public void OccupyGridForSingleBlock (Transform block) {
		int x = (int) block.position.Rounded ().x;
		int y = (int) block.position.Rounded ().y;
		this [x, y] = block;
	}

	public void FreeGridForTetromino (Transform tetromino) {
		foreach (Transform block in tetromino) {
			FreeGridForSingleBlock (block);
		}
	}

	public void FreeGridForSingleBlock (Transform block) {
		int x = (int) block.position.Rounded ().x;
		int y = (int) block.position.Rounded ().y;
		this [x, y] = null;
	}

	public void DestroyRow (int rowIndex) {

		// Destroy row
		for (int col = 0; col < Width; col++) {
			if (this [col, rowIndex] == null) {
				continue;
			}

			GameObject.Destroy (this [col, rowIndex].gameObject);
			this [col, rowIndex] = null;
		}

		// Lower each block by 1 row.
		for (int row = rowIndex + 1; row < Height; row++) {
			for (int col = 0; col < Width; col++) {
				if (this [col, row] == null) {
					continue;
				}

				var block = this [col, row];

				FreeGridForSingleBlock (block);
				block.Move (MoveDirection.Down);
				OccupyGridForSingleBlock (block);
			}
		}
	}
}

public class GridManager : MonoBehaviour {
	public int Width;
	public int Height;

	public Grid Grid;

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

		Grid = new Grid (width, heightWithPadding);

		MoveEventsQueue = new Queue<GameEvent> ();

		EventManager.Instance.AddListener<MoveTetrominoEvent> (EnqueueMoveTetrominoEvent);
		EventManager.Instance.AddListener<RotateTetrominoEvent> (EnqueueRotateTetrominoEvent);
		EventManager.Instance.AddListener<TetrominoLandedEvent> (EnqueueTetrominoLandedEvent);
		EventManager.Instance.AddListener<RowsFullEvent> (RowsFullEventHandler);
	}

	void Update () {

		if (MoveEventsQueue.Count () > 0) {
			ProcessQueueItems ();
		}
	}

	private void EnqueueMoveTetrominoEvent (MoveTetrominoEvent e) {
		MoveEventsQueue.Enqueue (e);
	}

	private void EnqueueRotateTetrominoEvent (RotateTetrominoEvent e) {
		MoveEventsQueue.Enqueue (e);
	}

	private void EnqueueTetrominoLandedEvent (TetrominoLandedEvent e) {
		MoveEventsQueue.Enqueue (e);
	}

	private void ProcessQueueItems () {
		GameEvent e = MoveEventsQueue.Dequeue ();

		switch (e.GetType ().Name) {
			case "MoveTetrominoEvent":
				ValidateMoveEventHandler ((MoveTetrominoEvent) e);
				break;
			case "RotateTetrominoEvent":
				ValidateRotationEventHandler ((RotateTetrominoEvent) e);
				break;
			case "TetrominoLandedEvent":
				TetrominoLandedEventHandler ((TetrominoLandedEvent) e);
				break;
		}
	}

	private void RowsFullEventHandler (RowsFullEvent e) {
		var sorted = e.RowIndexes.Cast<int> ().OrderByDescending (i => i);

		foreach (int i in sorted) {
			Debug.LogFormat ("Destroying row {0}", i);
			Grid.DestroyRow (i);
		}
	}

	// Print the grid as a series of 0s and 1s for debugging.
	public void Inspect () {
		var output = "";
		for (int row = Height; row >= 0; row--) {
			output += "\n";
			for (int col = 0; col < Width; col++) {
				var bit = Grid[col, row] == null ? "0" : "1";
				output += bit;
			}
		}

		DebugCanvas canvas = GameObject.FindObjectOfType (typeof (DebugCanvas)) as DebugCanvas;
		canvas.SetText (output);
	}

	public void TetrominoLandedEventHandler (TetrominoLandedEvent e) {
		if (FullRowIndexes ().Length > 0) {
			var indexes = FullRowIndexes ();
			Debug.LogFormat ("Full row indexes: {0}", indexes);
			EventManager.Instance.TriggerEvent (new RowsFullEvent (indexes));
		}
	}

	int[] FullRowIndexes () {
		List<int> fullRowIndexes = new List<int> ();
		for (int row = 0; row < Height; row++) {
			List<bool> blocksInRow = new List<bool> ();
			for (int col = 0; col < Width; col++) {
				blocksInRow.Add (Grid[col, row] != null);
			}

			if (blocksInRow.All (value => value == true)) {
				fullRowIndexes.Add (row);
			}
		}

		return fullRowIndexes.ToArray ();
	}

	// TODO: move this to a separate class.
	// **************
	// Validate moves
	// **************

	public void ValidateMoveEventHandler (MoveTetrominoEvent e) {
		ValidateMove (e.CurrentPosition, e.Direction);
	}

	public void ValidateRotationEventHandler (RotateTetrominoEvent e) {
		ValidateRotation (e.CurrentPosition, e.Direction);
	}

	public void ValidateMove (Transform currentPosition, MoveDirection direction) {
		Transform[, ] initalGridState = new Transform[Grid.Width, Grid.Height];
		System.Array.Copy (Grid.Value, initalGridState, Grid.Width * Grid.Height);

		Grid.FreeGridForTetromino (currentPosition);

		bool isValid = AssertValidTetrominoMove (currentPosition, DirectionToVector.For (direction));

		if (isValid) {
			EventManager.Instance.QueueEvent (new MoveValidEvent (currentPosition, direction));
		} else {
			Grid.Value = initalGridState;
			EventManager.Instance.QueueEvent (new MoveInvalidEvent (currentPosition, direction));
		}
	}

	public void ValidateRotation (Transform currentPosition, RotateDirection direction) {
		Transform[, ] initalGridState = new Transform[Grid.Width, Grid.Height];
		System.Array.Copy (Grid.Value, initalGridState, Grid.Width * Grid.Height);

		Grid.FreeGridForTetromino (currentPosition);

		bool isValid = AssertValidTetrominoRotation (currentPosition, direction);

		if (isValid) {
			EventManager.Instance.QueueEvent (new RotateValidEvent (currentPosition, direction));
		} else {
			Grid.Value = initalGridState;
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
			x < Grid.Width &&
			Grid[x, y] == null;
	}
}