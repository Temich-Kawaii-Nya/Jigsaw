using UnityEngine;

public class GridService
{
    private UnityEngine.Grid _sceneGrid; //TODO убрать юнитовскую сетку
    public GridProxy GridModel { get; }
    private readonly EventBus _eventBus;
    private Rect _gridRect;
    private Rect _maxGridRect;
    public GridService(UnityEngine.Grid sceneGrid, GridProxy gridProxy, EventBus eventBus)
    {
        _sceneGrid = sceneGrid;
        GridModel = gridProxy;
        _eventBus = eventBus;

        _sceneGrid.cellSize = gridProxy.gridSize;

        SetRect();
    }
    private void SetRect()
    {
        Vector2 pos = new (GridModel.Size.y * GridModel.gridSize.x, GridModel.Size.x * GridModel.gridSize.y);
        _gridRect = new(0, 0, pos.x, pos.y);

        Vector2 leftBot = Camera.main.ScreenToWorldPoint(new Vector3(0,0,-1));
        Vector2 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -1));

        _maxGridRect = new Rect(leftBot.x, leftBot.y, rightTop.x, rightTop.y);

        Debug.Log("MinGrid: " + _gridRect);
        Debug.Log("MaxGrid: " + _maxGridRect);
    }
    public Vector3 GetRandomPositionOutSideTheGrid()
    {
        int side = Random.Range(0, 4);
        float x;
        float y;
        switch (side)
        {
            case 0:
                x = Random.Range(_maxGridRect.x + GridModel.gridSize.x / 2f, _gridRect.x - GridModel.gridSize.x / 2f);
                y = Random.Range(_maxGridRect.y + GridModel.gridSize.y / 2f, _maxGridRect.height - GridModel.gridSize.y / 2f); //TODO 
                break;
            case 1:
                x = Random.Range(_maxGridRect.x + GridModel.gridSize.x / 2f, _maxGridRect.width - GridModel.gridSize.x / 2f);
                y = Random.Range(_gridRect.height + GridModel.gridSize.y / 2f, _maxGridRect.height - GridModel.gridSize.y / 2f); 
                break;
            case 2:
                x = Random.Range(_gridRect.width + GridModel.gridSize.x / 2f, _maxGridRect.width - GridModel.gridSize.x / 2f);
                y = Random.Range(_maxGridRect.y + GridModel.gridSize.y / 2f, _maxGridRect.height - GridModel.gridSize.y / 2f); 
                break;
            case 3:
                x = Random.Range(_maxGridRect.x + GridModel.gridSize.x / 2f, _maxGridRect.width - GridModel.gridSize.x / 2f);
                y = Random.Range(_maxGridRect.y + GridModel.gridSize.y / 2f, _gridRect.y - GridModel.gridSize.y / 2f);
                break;
            default:
                x = 0;
                y = 0;
                break;
        }
        return new Vector3(x, y, 10);
    }
    public Vector3 GetCellCenterPosFromWordlPos(Vector3 position)
    {
        Vector3 cell = GetCellFromWorldPosition(position);
        if (!IsPositionInGrid(position))
            return position;
        return GetWorldPositionFromIndex((int)cell.x, (int)cell.y);
    }
    public bool IsPositionInGrid(Vector3 position)
    {
        Vector3 cell = GetCellFromWorldPosition(position);
        if (cell.x < 0 || cell.y < 0 || cell.x >= GridModel.Size.x || cell.y >= GridModel.Size.y)
            return false;
        return true;
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
