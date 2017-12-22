using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private GameManager instance;
	private GameObject WelcomeCanvas;
	// private TetrisManager tetrisManager;

	void Awake () {
		if (instance == null)
			instance = new GameManager ();

		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		if (isTitleScene ()) {
			WelcomeCanvas = Instantiate (Resources.Load ("Prefabs/WelcomeCanvas") as GameObject);
			print ("Title scene");

		}

		if (isPlayScene ()) {
			print ("Play scene");
		}

	}

	// Update is called once per frame
	void Update () {
		if (isTitleScene () && Input.GetKeyDown (KeyCode.Return)) {
			StartGame ();
		}
	}

	void StartGame () {
		StartCoroutine (LoadPlayScene ());
	}

	IEnumerator LoadPlayScene () {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync ("_Scenes/play");
		while (!asyncLoad.isDone) {
			yield return null;
		}
		Destroy (WelcomeCanvas);
	}

	bool isTitleScene () {
		return SceneManager.GetActiveScene ().name == "title_screen";
	}

	bool isPlayScene () {
		return SceneManager.GetActiveScene ().name == "play";
	}
}