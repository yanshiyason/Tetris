using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Grid {
    public Transform[, ] Value;
    public int Width;
    public int Height;
    public int HeightWithPadding;

    public Transform this [int col, int row] {
        get { return Value[col, row]; }
        set { Value.SetValue (value, col, row); }
    }

    public Grid Copy () {
        var grid = new Grid (Width, Height);
        System.Array.Copy (Value, grid.Value, Width * HeightWithPadding);
        return grid;
    }

    public Grid (int width, int height) {
        Width = width;
        Height = height;
        HeightWithPadding = height + 10;
        Value = new Transform[width, HeightWithPadding];
    }

    public void Move (Transform tetromino) {
        OccupyGridForTetromino (tetromino);
    }

    public void OccupyGridForTetromino (Transform tetromino) {
        foreach (Transform block in tetromino) {
            OccupyGridForSingleBlock (block);
        }
    }

    public void OccupyGridForSingleBlock (Transform block) {
        int x = (int) block.position.Rounded ().x;
        int y = (int) block.position.Rounded ().y;
        this [x, y] = block;
    }

    public void FreeGridForTetromino (Transform tetromino) {
        foreach (Transform block in tetromino) {
            FreeGridForSingleBlock (block);
        }
    }

    public void FreeGridForSingleBlock (Transform block) {
        int x = (int) block.position.Rounded ().x;
        int y = (int) block.position.Rounded ().y;
        this [x, y] = null;
    }

    public bool DidLose () {
        for (int col = 0; col < Width; col++) {
            if (this [col, Height - 2] != null) {
                return true;
            }
        }
        return false;
    }

    public IEnumerator DestroyRow (int rowIndex) {

        // Destroy row
        for (int col = 0; col < Width; col++) {
            if (this [col, rowIndex] == null) {
                continue;
            }

            Debug.LogFormat ("Calling destroy loop {0}, {1}", col, rowIndex);
            yield return new WaitForSeconds (0.05f);

            // Show explosion
            var explosion = GameObject.Instantiate (GridManager.Explosion, this [col, rowIndex].position, Quaternion.identity);
            var ps = explosion.GetComponent<ParticleSystem> ();
            var main = ps.main;
            main.startColor = this [col, rowIndex].GetComponent<Block> ().Color;
            GameObject.DestroyObject (this [col, rowIndex].gameObject);
            ps.Play ();
            GameObject.DestroyObject (ps.gameObject, ps.main.duration);

            this [col, rowIndex] = null;

            ScoreManager.Score += 1;
        }

        Debug.LogFormat ("Calling LowerAllRowsFrom");

        LowerAllRowsFrom (rowIndex);
    }

    void LowerAllRowsFrom (int rowIndex) {
        // Lower each block by 1 row.
        for (int row = rowIndex + 1; row < Height; row++) {
            for (int col = 0; col < Width; col++) {
                if (this [col, row] == null) {
                    continue;
                }

                var block = this [col, row];

                FreeGridForSingleBlock (block);
                block.Move (MoveDirection.Down);
                OccupyGridForSingleBlock (block);
            }
        }
    }

    public int[] FullRowIndexes () {
        List<int> fullRowIndexes = new List<int> ();
        for (int row = 0; row < Height; row++) {
            List<bool> blocksInRow = new List<bool> ();
            for (int col = 0; col < Width; col++) {
                blocksInRow.Add (this [col, row] != null);
            }

            if (blocksInRow.All (value => value == true)) {
                fullRowIndexes.Add (row);
            }
        }

        return fullRowIndexes.ToArray ();
    }

    // Print the grid as a series of 0s and 1s for debugging.
    public string Inspect () {
        var output = "";
        for (int row = Height; row >= 0; row--) {
            output += "\n";
            for (int col = 0; col < Width; col++) {
                var bit = this [col, row] == null ? "0" : "1";
                output += bit;
            }
        }

        return output;
    }
}