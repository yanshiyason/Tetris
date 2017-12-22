using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Tetris.Extensions;

public class GridManager {
	public bool[,] grid;
	public int width;
	public int height;

	public GridManager(int width = 12, int height = 24) {
		this.width = width;
		this.height = height;

        grid = new bool[width, height];

		// grid = Enumerable.Range(0, width).Select(
		// 	_row => Enumerable.Range(0, height).Select(_col => null).ToArray()
		// ).ToArray();
	}

	// public void SpawnRandom() {
	// 	var row = Random.Range(0, width);
	// 	var col = Random.Range(0, height);
	// 	SpawnInGrid(row, col);
	// }

	public bool AssertValidGroupMove(Transform group, Vector3 to)
	{
		bool[,] initalGridState = grid.Clone() as bool[,];

		IEnumerable<Transform> transforms = TetrisManager.fallingBlockGroup.transform.Cast<Transform>();

		foreach (var t in transforms)
		{
			grid.SetGridValue(false, t.position);
		}

		bool isValid = transforms.Select(t => AssertValidMove(t.position, to)).All(valid => valid == true);

		if (isValid) {			
			return true;
		} else {
			grid = initalGridState;
			return false;
		}
	}

	public bool AssertValidMove(Vector3 from, Vector3 to)
	{
		int x = (int)(from + to).x;
		int y = (int)(from + to).y;
		return AssertValid(x, y);
	}

    public bool AssertValid(int x, int y) {
        return grid[x,y] == false;
    }

	void SpawnInGrid(int x, int y) {
		var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = new Vector3(x, y, 0);
	}

	void Move(Transform group)
	{
		MarkgridAsOccupied(group.transform);
	}

	void MarkgridAsOccupied(Transform group)
	{
		foreach (Transform t in group) {
			grid.SetValue(true, (int)t.position.x, (int)t.position.y);
		}
		Inspect();
	}

	// Print the grid as a series of 0s and 1s for debugging.
	public void Inspect() {
		var output = "";
		for (int row = 0; row < width; row++ ) {
			output += "\n";
			for (int col = 0; col < height; col++) {
				var bit = grid[row,col] ? "1" : "0";
				output += bit;
			}
		}
		Debug.Log(output);
	}
}