using UnityEngine;
using Zenject;

public class PuzzleStates
{
    public PuzzleIdleState PuzzleIdleState { get; private set; }
    public PuzzleMoveState PuzzleMoveState { get; private set; }
    [Inject]
    public PuzzleStates(
        PuzzleMoveState move)
    {
        PuzzleIdleState = new();
        PuzzleMoveState = move;
    }    
}
