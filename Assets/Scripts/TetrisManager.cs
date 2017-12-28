using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public class TetrisManager : MonoBehaviour {
	public static TetrisManager Instance { get { return _instance; } }

	GridManager GridManager;

	private static TetrisManager _instance;
	private Object[] shapes;

	public GameObject CurrentlyFallingTetromino;

	void Awake () {
		SetupSingleton ();
		// Setup event system.
		gameObject.AddComponent<EventManager> ();

		shapes = Resources.LoadAll ("Prefabs/Shapes", typeof (GameObject));

		// Setup GridManager
		GridManager = FindObjectOfType<GridManager> ();

		if (GridManager == null) {
			throw new GridManagerNotFoundException ();
		}

		GridManager.Initialize (10, 22);

		// Listen for Spawn Tetromino events
		EventManager.Instance.AddListener<SpawnTetrominoEvent> (SpawnTetrominoEventHandler);

		SpawnTetromino ();
	}

	// Use this for initialization
	void Start () {
		DrawWallsAroundTetrisGrid ();
	}

	// Update is called once per frame
	void Update () { }

	void SpawnTetrominoEventHandler (SpawnTetrominoEvent _) {
		SpawnTetromino ();
	}

	void SpawnTetromino () {
		int x = (int) Mathf.Round (GridManager.Instance.Width / 2);
		int y = GridManager.Instance.Height;
		Debug.LogFormat ("Spawning x: {0}, y: {1}", x, y);
		var spawnPoint = new Vector3 (x, y, 0);
		SpawnTetromino (spawnPoint);
	}

	void SpawnTetromino (Vector3 position) {
		var shapePrefab = (GameObject) shapes[Random.Range (0, shapes.Length)];
		CurrentlyFallingTetromino = Instantiate (shapePrefab, position, Quaternion.identity);
	}

	private void SetupSingleton () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
	}

	private void DrawWallsAroundTetrisGrid () {
		int width = GridManager.Width;
		int height = GridManager.Height;

		// Bottom wall
		for (int i = 0; i < width; i++) {
			SpawnWallCube (new Vector3 (i, -1, 0));
		}

		// Left wall
		for (int i = -1; i < height; i++) {
			SpawnWallCube (new Vector3 (-1, i, 0));
		}

		// Right wall
		for (int i = -1; i < height; i++) {
			SpawnWallCube (new Vector3 (width, i, 0));
		}
	}

	void SpawnWallCube (Vector3 position) {
		var cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.position = position;
		cube.GetComponent<Renderer> ().material.color = Color.black;
	}
}