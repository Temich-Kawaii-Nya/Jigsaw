using System;
using UnityEngine;

[Serializable]
public class Grid
{
    public Vector2Int size;
    public Vector2 cellSize;
    public PuzzleEntityProxy[,] puzzlesGrid;
    
    public Grid(Vector2Int size, Vector2 cellSize)
    {
        this.size = size;
        this.cellSize = cellSize;
        puzzlesGrid = new PuzzleEntityProxy[this.size.x, this.size.y];
    }
}
