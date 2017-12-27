using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TetrominoTest {

	static GameObject go;

	[SetUp] public void Init () {
		go = new GameObject ();
		go.AddComponent<Tetromino> ();
	}

	[TearDown] public void Dispose () { }

	[UnityTest] public IEnumerator Tetromino_WithRigidBody_WillNotBeAffectedByPhysics () {
		var go = new GameObject ();
		go.AddComponent<Tetromino> ();

		var originalPosition = go.transform.position.y;

		yield return new WaitForFixedUpdate ();

		Assert.AreEqual (originalPosition, go.transform.position.y);
	}

	[UnityTest] public IEnumerator Tetromino_Has_A_PlayerInputListenerComponent_Attached () {
		yield return null;
		var playInputListener = go.GetComponent<PlayerInputListener> ();
		Assert.True (playInputListener != null);
	}
}