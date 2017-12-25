using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour {
	Text text;

	// Use this for initialization
	void Awake () {
		text = GetComponentInChildren<Text> ();
	}

	public void SetText (string newText) {
		text.text = newText;
	}
}