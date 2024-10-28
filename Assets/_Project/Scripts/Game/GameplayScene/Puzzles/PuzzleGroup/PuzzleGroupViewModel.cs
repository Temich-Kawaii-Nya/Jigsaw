using ObservableCollections;
using R3;
using UnityEngine;
public class PuzzleGroupViewModel
{
    private readonly PuzzleService _puzzleService;
    public readonly ObservableList<PuzzleEntityProxy> _puzzles = new();
    public PuzzleGroupEntityProxy _proxy;
    public ReadOnlyReactiveProperty<int> Rotation { get; private set; }
    public ReadOnlyReactiveProperty<Vector3> Position { get; private set; }
    public PuzzleGroupViewModel(
        PuzzleGroupEntityProxy proxy,
        PuzzleService puzzleService)
    {
        _proxy = proxy;
        _puzzleService = puzzleService;
        Rotation = proxy.Rotation;
        Position = proxy.Position;
        _puzzles = proxy._puzzles;
    }
    public void AddPuzzleToGroup(PuzzleEntityProxy proxy)
    {
        _puzzles.Add(proxy);
        proxy.Parent.Value = _proxy;
    }
    public void RemovePuzzleFromGroup(PuzzleEntityProxy id) 
    {
        _puzzles.Remove(id);
    }
    public void Move(Vector3 position)
    {
        _puzzleService.MoveGroupToPosition(_proxy, position);
    }
    public void Rotate() 
    {
        _puzzleService.RotatePuzzleGroup(_proxy);
    }
    public void Drop()
    {
        for (int i = 0; i < _puzzles.Count; i++)
        {
            _puzzleService.CheckPuzzlePosition(_puzzles[i].Id);
        }
        for (int i = 0; i < _puzzles.Count; i++)
        {
            _puzzleService.CheckCombinations(_puzzles[i]);
        }
    }
    public void DoAfterRotation()
    {
        _puzzleService.DoAfterRotatePuzzlesGroup(_proxy);
    }
}
