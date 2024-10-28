using R3;

public interface IGameStateProvider 
{
    public PuzzleStateProxy PuzzleState { get; }
    public GridStateProxy GridState { get; }

    public Observable<PuzzleStateProxy> LoadPuzzleState();
    public Observable<bool> SavePuzzleState();
    public Observable<bool> ResetPuzzleState();
    public Observable<GridStateProxy> LoadGridState();
    public Observable<bool> SaveGridState();
    public Observable<bool> ResetGridState();
}
