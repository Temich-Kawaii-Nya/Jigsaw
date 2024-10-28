using UnityEngine;
using Zenject;

public class Puzzle : MonoBehaviour
{
    [Inject] 
    public PuzzleStateMachine StateMachine { get; private set; }
    
    private void Start()
    {
        StateMachine.ChangeState(StateMachine.PuzzleMoveState);
    }
    private void Update()
    {
        StateMachine.Update();
    }
    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
}
