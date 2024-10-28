using Zenject;

public class PuzzleStateMachine : StateMachine
{
    public Puzzle Puzzle { get; private set; }
    public PuzzleIdleState PuzzleIdleState { get; private set; }
    public PuzzleMoveState PuzzleMoveState { get; private set; }


    public PuzzleStateMachine(
        Puzzle puzzle,
        PuzzleIdleState idle,
        PuzzleMoveState move)
    {
        Puzzle = puzzle;
        PuzzleMoveState = move;
        PuzzleIdleState = idle;
    }
}
