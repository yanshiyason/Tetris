using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridTest {
    [SetUp] public void Init () { }

    [TearDown] public void Dispose () { }

    [Test]
    public void Grid_Initializes_With_The_Correct_Amount_Of_Cells () {
        var width = 10;
        var height = 20;
        var Grid = new Grid (width, height);
        Assert.AreEqual (Grid.Width, width);
        Assert.AreEqual (Grid.Height, height);
    }
}