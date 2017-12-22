using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockGroup : MonoBehaviour {
	Rigidbody rigidbody;

	// Use this for initialization
	void Awake() {
		rigidbody = this.gameObject.AddComponent<Rigidbody>();
		rigidbody.useGravity = false;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
