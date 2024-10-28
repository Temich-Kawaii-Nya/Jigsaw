using UnityEngine;
using Zenject;

public class PuzzleMoveState : IState
{
    private InputService _inputService;
    private Puzzle _puzzle;
    [Inject]
    private void Construct(
        InputService input,
        Puzzle puzzle)
    {
        _inputService = input;
        _puzzle = puzzle;
    }

    public void Enter()
    {
        Debug.Log("Move");
    }

    public void Exit()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(_inputService.MousePos.x, _inputService.MousePos.y, 20));
        pos.y = 0;
        _puzzle.transform.position = Vector3.MoveTowards(_puzzle.transform.position, pos, 25 * Time.deltaTime);
    }
}
