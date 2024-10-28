using UnityEngine;
using Zenject;

public class PuzzleInstaller : MonoInstaller
{
    [SerializeField] private Puzzle puzzle;
    public override void InstallBindings()
    {
        Container.Bind<PuzzleStateMachine>().AsSingle();
        Container.Bind<PuzzleIdleState>().AsSingle();
        Container.Bind<PuzzleMoveState>().AsSingle(); 
    }
}
