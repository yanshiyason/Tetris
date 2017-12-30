using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
	Text text;

	// Use this for initialization
	void Awake () {
		text = GetComponent<Text> ();
	}

	string Text;

	void Update () {
		var gridRepresentation = GridManager.Instance.Grid.Inspect ();
		if (Text != gridRepresentation) {
			SetText (gridRepresentation);

			Text = gridRepresentation;
		}

	}

	public void SetText (string text) {
		this.text.text = text;
	}
}