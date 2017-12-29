using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridTest {
	static GameObject go;

	[SetUp] public void Init () {
		go = new GameObject ();
		go.AddComponent<GridManager> ();
	}

	[TearDown] public void Dispose () { }

	[Test]
	public void Grid_Initializes_With_The_Correct_Amount_Of_Cells () {
		var heightPadding = 10;
		var width = 24;
		var height = 48;
		var totalCells = height * width;
		GridManager.Instance.Initialize (width, height);
		Assert.AreEqual (GridManager.Instance.Grid.Width, width);
		Assert.AreEqual (GridManager.Instance.Grid.Height, height + heightPadding);
	}
}