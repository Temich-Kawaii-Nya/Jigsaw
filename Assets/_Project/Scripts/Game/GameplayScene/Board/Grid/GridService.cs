using UnityEngine;

public class GridService
{
    private UnityEngine.Grid _sceneGrid; //TODO убрать юнитовскую сетку
    public GridProxy GridModel { get; }
    private readonly EventBus _eventBus;

    public GridService(UnityEngine.Grid sceneGrid, GridProxy gridProxy, EventBus eventBus)
    {
        _sceneGrid = sceneGrid;
        GridModel = gridProxy;
        _eventBus = eventBus;

        _sceneGrid.cellSize = gridProxy.gridSize;
    }
    public Vector3 GetWorldPositionFromIndex(int i, int j)
    {
        return _sceneGrid.GetCellCenterWorld(new Vector3Int(i, j, 0));
    }
    public Vector3 GetCellFromWorldPosition(Vector3 position)
    {
        return _sceneGrid.WorldToCell(position);
    }
    public void InserPuzzleAtCell(PuzzleEntityProxy puzzle, int i, int j)
    {
        GridModel.puzzlesGrid[i, j] = puzzle;
        _eventBus.TriggerEvent(new ScoreIncreaseEvent(500)); //TODO Убрать хардкод config
        CheckGridOnComplete();
    }
    public void RemovePuzzleAtCell(int i, int j)
    {
        GridModel.puzzlesGrid[i, j] = null;
    }
    private void CheckGridOnComplete()
    {
        for (int i = 0; i < GridModel.puzzlesGrid.GetLength(0); i++)
        {
            for (int j = 0; j < GridModel.puzzlesGrid.GetLength(1); j++)
            {
                if (GridModel.puzzlesGrid[i, j] == null)
                    return;
            }
        }
        Debug.Log("Win");
    }
}
