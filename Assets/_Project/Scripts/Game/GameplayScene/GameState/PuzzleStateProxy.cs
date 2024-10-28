using R3;
using ObservableCollections;
using System.Linq;

public class PuzzleStateProxy
{
    public ObservableList<PuzzleEntityProxy> Puzzles { get; } = new();
    private readonly PuzzleState _puzzleState;
    public PuzzleStateProxy(PuzzleState puzzleState)
    {
        _puzzleState = puzzleState;

        puzzleState.puzzleEntities.ForEach(puzzleOrigin => Puzzles.Add(new PuzzleEntityProxy(puzzleOrigin)));

        Puzzles.ObserveAdd().Subscribe(e =>
        {
            var addedPuzzleEntity = e.Value;
            puzzleState.puzzleEntities.Add(new PuzzleEntity
            {
                id = addedPuzzleEntity.Id,
                index = addedPuzzleEntity.Index,
                position = addedPuzzleEntity.Position.Value,
                Rotation = addedPuzzleEntity.Rotation.Value
            });
        });

        Puzzles.ObserveRemove().Subscribe(e =>
        {
            var removedPuzzleEntityProxy = e.Value;
            var removedPuzzleEntity = puzzleState.puzzleEntities.FirstOrDefault(b => b.id == removedPuzzleEntityProxy.Id);
            puzzleState.puzzleEntities.Remove(removedPuzzleEntity);
        });
    }
    public int GetPuzzleId()
    {
        return _puzzleState.GlobalPuzzleId++;
    }
}
