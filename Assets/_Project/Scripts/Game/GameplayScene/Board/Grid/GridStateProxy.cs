using ObservableCollections;
using R3;

public class GridStateProxy 
{
    public ObservableList<PuzzleEntityProxy> GridPuzzles { get; } = new();
    private readonly GridState _originalState;

    public GridStateProxy(GridState gridState)
    {
        _originalState = gridState;
        for (var i = 0; i < _originalState.grid.puzzlesGrid.GetLength(0); i++) 
        {
            for (var j = 0; j < _originalState.grid.puzzlesGrid.GetLength(1); j++)
            {
                if (_originalState.grid.puzzlesGrid[i, j] != null)
                    GridPuzzles.Add(_originalState.grid.puzzlesGrid[i, j]);
            }
        }
        GridPuzzles.ObserveAdd().Subscribe(e =>
        {
            var addedPuzzleEntity = e.Value;

            _originalState.grid.puzzlesGrid[addedPuzzleEntity.Index.x, addedPuzzleEntity.Index.y] = addedPuzzleEntity;
        });

        GridPuzzles.ObserveRemove().Subscribe(e =>
        {
            var removedPuzzleEntityProxy = e.Value;
            gridState.grid.puzzlesGrid[removedPuzzleEntityProxy.Index.x, removedPuzzleEntityProxy.Index.y] = null;
        });
    }
}
