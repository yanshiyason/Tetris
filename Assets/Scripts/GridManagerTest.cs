using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GridTest {

	[Test]
	public void Grid_Initializes_With_The_Correct_Amount_Of_Cells() {
		var width = 24;
		var height = 48;
		var totalCells = height * width;
		var grid = new Grid(24, 48);
		Assert.AreEqual(grid.cells.GetLength(0), 24);
		Assert.AreEqual(grid.cells.GetLength(1), 48);
	}

	[Test]
	public void GridTestSimplePasses() {
	}


	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator GridTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
