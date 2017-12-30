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

	void Awake () {
		SetupSingleton ();
		// Setup event system.
		gameObject.AddComponent<EventManager> ();

		// Setup GridManager
		GridManager = FindObjectOfType<GridManager> ();

		if (GridManager == null) {
			throw new GridManagerNotFoundException ();
		}

		GridManager.Initialize (10, 20);

		EventManager.Instance.AddListener<GameOverEvent> (GameOverEventHandler);
	}

	// Use this for initialization
	void Start () {
		DrawWallsAroundTetrisGrid ();
	}

	void GameOverEventHandler (GameOverEvent e) {
		print ("GaAME OVER!");
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
		int width = GridManager.Instance.Grid.Width;
		int height = GridManager.Instance.Grid.Height;

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