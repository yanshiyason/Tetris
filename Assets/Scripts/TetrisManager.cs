﻿using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public class TetrisManager : MonoBehaviour {
	public static TetrisManager instance { get { return _instance; } }
	public static GameObject fallingBlockGroup;

	private static TetrisManager _instance;
	private Object[] shapes;
	public static GridManager GridManager;

	void Awake () {
		SetupSingleton ();
		// Setup event system.
		gameObject.AddComponent<EventManager> ();

		shapes = Resources.LoadAll ("Prefabs/Shapes", typeof (GameObject));

		// Setup GridManager
		GridManager = gameObject.AddComponent<GridManager> ();
		GridManager.Initialize (12, 24);

		// Setup Input Listener
		gameObject.AddComponent<PlayerInputListener> ();
	}

	// Use this for initialization
	void Start () {
		GridManager.Inspect ();
	}

	// Update is called once per frame
	void Update () {
		if (fallingBlockGroup == null) {
			SpawnShape ();
		}
	}

	void SpawnShape () {
		int x = (int) Mathf.Round (GridManager.Width / 2);
		int y = GridManager.Height;
		Debug.LogFormat ("Spawning x: {0}, y: {1}", x, y);
		var spawnPoint = new Vector3 (x, y, 0);
		var shapePrefab = (GameObject) shapes[Random.Range (0, shapes.Length)];
		fallingBlockGroup = Instantiate (shapePrefab, spawnPoint, Quaternion.identity);
		fallingBlockGroup.AddComponent<FallingBlockGroup> ();
	}

	private void SetupSingleton () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
	}
}