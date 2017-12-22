using System.Collections;
using System.Collections.Generic;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public class TetrisManager : MonoBehaviour {
	private static TetrisManager _instance;
	public static TetrisManager instance {
		get { return _instance; }
	}
	private Object[] shapes;
	public static GridManager gridManager;
	public static GameObject fallingBlockGroup;

	void Awake () {
		SetupSingleton ();
		shapes = Resources.LoadAll ("Prefabs/Shapes", typeof (GameObject));
		gridManager = new GridManager ();
		gameObject.AddComponent<EventManager> ();
		gameObject.AddComponent<PlayerInputListener> ();
	}

	// Use this for initialization
	void Start () {
		gridManager.Inspect ();
	}

	// Update is called once per frame
	void Update () {
		if (fallingBlockGroup == null) {
			SpawnShape ();
		}
	}

	void SpawnShape () {
		var spawnPoint = new Vector3 (Mathf.Round (gridManager.width / 2), gridManager.height - 1, 0);
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