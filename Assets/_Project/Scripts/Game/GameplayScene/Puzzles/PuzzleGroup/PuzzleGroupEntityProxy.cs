using ObservableCollections;
using R3;
using UnityEngine;

public class PuzzleGroupEntityProxy
{
    public ReactiveProperty<int> Rotation { get; }
    public ObservableList<PuzzleEntityProxy> _puzzles { get; }
    public ReactiveProperty<Vector3> Position { get; }
    private readonly PuzzleGroupEntity _origin;
    public PuzzleGroupEntityProxy(PuzzleGroupEntity origin)
    {
        _origin = origin;

        Rotation = new ReactiveProperty<int>(origin.rotation);
        _puzzles = new();
        _puzzles.ObserveAdd().Subscribe(e =>
        {
            origin.puzzlesID.Add(e.Value.Id);
        });
        Rotation.Skip(1).Subscribe(e =>
        {
            origin.rotation = e;
        });
        Position = new ReactiveProperty<Vector3>(origin.position);
        Position.Subscribe(e =>
        {
            origin.position = e;
        });
    }
}
