using R3;
using UnityEngine;

public class PuzzleEntityProxy
{
    public int Id { get; }
    public ReactiveProperty<Vector3> Position { get; }
    public ReactiveProperty<int> Rotation { get; }
    public Vector2Int Index { get; }
    public bool IsCombined = false;

    public ReactiveProperty<PuzzleGroupEntityProxy> Parent { get; }

    public PuzzleEntityProxy(PuzzleEntity puzzle)
    {
        Id = puzzle.id;
        Index = puzzle.index;

        Position = new ReactiveProperty<Vector3>(puzzle.position);
        Rotation = new ReactiveProperty<int>(puzzle.Rotation);

        Position.Skip(1).Subscribe(value => puzzle.position = value);
        Rotation.Skip(1).Subscribe(value => puzzle.Rotation = value);

        Parent = new ReactiveProperty<PuzzleGroupEntityProxy>(null);
    }
}
