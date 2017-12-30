using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {
	public Grid Grid;

	public GameObject CurrentlyFallingTetromino;

	private Object[] Shapes;
	public static GameObject Explosion;

	private static GridManager _instance;

	private int heightWithPadding;

	public static GridManager Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof (GridManager)) as GridManager;
			}
			return _instance;
		}
	}

	void Start () { }

	public void Initialize (int width = 12, int height = 24) {
		Grid = new Grid (width, height);
		EventManager.Instance.AddListener<MoveTetrominoEvent> (MoveTetrominoEventHandler);
		EventManager.Instance.AddListener<RotateTetrominoEvent> (RotateTetrominoEventHandler);
		EventManager.Instance.AddListener<TetrominoLandedEvent> (TetrominoLandedEventHandler);
		EventManager.Instance.AddListener<RowsFullEvent> (RowsFullEventHandler);

		Shapes = Resources.LoadAll ("Prefabs/Shapes", typeof (GameObject));
		Explosion = Resources.Load ("Prefabs/Explosion", typeof (GameObject)) as GameObject;

		if (CurrentlyFallingTetromino == null) {
			SpawnTetromino ();
		}
	}

	private void RowsFullEventHandler (RowsFullEvent e) {
		StartCoroutine (ProcessFullRows (e));
	}

	IEnumerator ProcessFullRows (RowsFullEvent e) {
		var sorted = e.RowIndexes.Cast<int> ().OrderByDescending (i => i);
		foreach (int i in sorted) {
			Debug.LogFormat ("Destroying row {0}", i);
			yield return StartCoroutine (Grid.DestroyRow (i));
		}

		SpawnTetromino ();
	}

	public void TetrominoLandedEventHandler (TetrominoLandedEvent e) {
		if (Grid.FullRowIndexes ().Length > 0) {
			RemoveFullRows ();
		} else {
			SpawnTetromino ();
			CheckGameOver ();
		}
	}

	void RemoveFullRows () {
		var indexes = Grid.FullRowIndexes ();
		Debug.LogFormat ("Full row indexes: {0}", indexes);
		EventManager.Instance.TriggerEvent (new RowsFullEvent (indexes));
	}

	void CheckGameOver () {
		if (Grid.DidLose ()) {
			EventManager.Instance.TriggerEvent (new GameOverEvent ());
		}
	}

	void SpawnTetromino () {
		int x = (int) Mathf.Round (Grid.Width / 2);
		int y = Grid.Height;
		Debug.LogFormat ("Spawning x: {0}, y: {1}", x, y);
		var spawnPoint = new Vector3 (x, y, 0);
		SpawnTetromino (spawnPoint);
	}

	void SpawnTetromino (Vector3 position) {
		var shapePrefab = (GameObject) Shapes[Random.Range (0, Shapes.Length)];
		CurrentlyFallingTetromino = Instantiate (shapePrefab, position, Quaternion.identity);
	}

	void MoveTetrominoEventHandler (MoveTetrominoEvent e) {
		ValidateMove (e.CurrentPosition, e.Direction);
	}

	void RotateTetrominoEventHandler (RotateTetrominoEvent e) {
		ValidateRotation (e.CurrentPosition, e.Direction);
	}

	public void ValidateMove (Transform currentPosition, MoveDirection direction) {
		Grid initalGrid = Grid.Copy ();

		Grid.FreeGridForTetromino (currentPosition);

		bool isValid = AssertValidTetrominoMove (currentPosition, DirectionToVector.For (direction));

		if (isValid) {
			EventManager.Instance.TriggerEvent (new MoveValidEvent (currentPosition, direction));
		} else {
			Grid = initalGrid;
			EventManager.Instance.TriggerEvent (new MoveInvalidEvent (currentPosition, direction));
		}
	}

	public void ValidateRotation (Transform currentPosition, RotateDirection direction) {
		Grid initalGrid = Grid.Copy ();

		Grid.FreeGridForTetromino (currentPosition);

		bool isValid = AssertValidTetrominoRotation (currentPosition, direction);

		if (isValid) {
			EventManager.Instance.TriggerEvent (new RotateValidEvent (currentPosition, direction));
		} else {
			Grid = initalGrid;
			EventManager.Instance.TriggerEvent (new RotateInvalidEvent (currentPosition, direction));
		}
	}

	public bool AssertValidTetrominoMove (Transform tetromino, Vector3 to) {
		foreach (Transform block in tetromino) {
			if (AssertValidMove (block.position, to)) {
				continue;
			}
			return false;
		}

		return true;
	}

	public bool AssertValidTetrominoRotation (Transform tetromino, RotateDirection direction) {
		// start by rotating the parent
		tetromino.RotateInDirection (direction);

		// Get all child transforms
		IEnumerable<Transform> transforms = tetromino.Cast<Transform> ();

		// Validate
		bool isValid = transforms.Select (block => AssertValidPosition (block.position)).All (valid => valid == true);

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