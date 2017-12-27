using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridTest {

	[Test]
	public void Grid_Initializes_With_The_Correct_Amount_Of_Cells () {
		var heightPadding = 10;
		var width = 24;
		var height = 48;
		var totalCells = height * width;
		var gridManager = new GridManager ();
		gridManager.Initialize (width, height);
		Assert.AreEqual (gridManager.Grid.GetLength (0), width);
		Assert.AreEqual (gridManager.Grid.GetLength (1), height + heightPadding);
	}

	[Test]
	public void GridTestSimplePasses () { }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator GridTestWithEnumeratorPasses () {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}