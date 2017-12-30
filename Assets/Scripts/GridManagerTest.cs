using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridManagerTest {
	static GameObject go;

	[SetUp] public void Init () {
		go = new GameObject ();
		go.AddComponent<GridManager> ();
	}

	[TearDown] public void Dispose () { }

	[UnityTest] public IEnumerator GridManager_Tetromino_IsInstantiated_AfterFirstFrame () {
		yield return null;

		Assert.True (GridManager.Instance.CurrentlyFallingTetromino != null);
	}
}