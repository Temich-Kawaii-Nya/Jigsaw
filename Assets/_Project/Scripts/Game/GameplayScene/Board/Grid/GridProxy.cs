using UnityEngine;

public class GridProxy
{
    public Vector2Int Size { get; }

    public Vector2 gridSize { get; }

    public PuzzleEntityProxy[,] puzzlesGrid { get; }
    public GridProxy(Grid grid)
    {
        Size = grid.size;
        gridSize = grid.cellSize;
        puzzlesGrid = grid.puzzlesGrid;
    }
}
