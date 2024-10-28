using UnityEngine;
using Zenject;

public class PuzzlesGenerator 
{
    private GridService _gridService;
    private PuzzleService _puzzleService;
    private GameplayEnterParamsAdapter _gameplayEnterParams;

    [Inject]
    public void Construct(
        GridService gridService,
        PuzzleService puzzleService,
        GameplayEnterParamsAdapter gameplayEnterParams)
    {
        _gridService = gridService;
        _puzzleService = puzzleService;
        _gameplayEnterParams = gameplayEnterParams;
    }

    public void Generate()
    {
        Vector2Int size = _gameplayEnterParams.GridSize;
        for (int i = size.x - 1; i >= 0; i--)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 newPos = _gridService.GetWorldPositionFromIndex(j, i);
                int rawIndex = GetRawIndex(i, j, size);
                var puzzle = _puzzleService.CreatePuzzleWithIndex(new Vector2Int(j, i));
                _puzzleService.MovePuzzleToPosition(puzzle.Id, newPos);
            }
        }
    }
    private int GetRawIndex(int i, int j, Vector2Int size)
    {
        return i * size.y + (j + 1);
    }
}
