using R3;
using UnityEngine;

public class PuzzleViewModel 
{
    private readonly PuzzleEntityProxy _puzzleProxy;
    private readonly PuzzleService _puzzleService;

    public readonly int PuzzleEntityId;
    public ReadOnlyReactiveProperty<Vector3> Position { get; }
    public ReadOnlyReactiveProperty<int> Rotation { get; }
    public ReadOnlyReactiveProperty<PuzzleGroupEntityProxy> Parent { get; } 
    public PuzzleViewModel(PuzzleEntityProxy puzzle, PuzzleService service)
    {
        PuzzleEntityId = puzzle.Id;

        _puzzleProxy = puzzle;
        _puzzleService = service;

        Position = puzzle.Position;
        Rotation = puzzle.Rotation;

        Parent = puzzle.Parent;

    }
    public void MovePuzzleToPosition(Vector3 position)
    {
        _puzzleService.MovePuzzleToPosition(_puzzleProxy.Id, position);
    }
    public void Rotate()
    {
        _puzzleService.RotatePuzzle(_puzzleProxy.Id);
    }
    public void Drop()
    {
        _puzzleService.CheckPuzzlePosition(_puzzleProxy.Id);
        _puzzleService.CheckCombinations(_puzzleProxy);
    }
}
