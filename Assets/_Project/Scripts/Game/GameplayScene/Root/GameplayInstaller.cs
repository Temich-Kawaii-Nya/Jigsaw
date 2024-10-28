using Zenject;

public class GameplayInstaller : Installer<GameplayInstaller>
{
    private IGameStateProvider _gameStateProvider;

    public GameplayInstaller()
    {
        _gameStateProvider = ProjectContext.Instance.Container.Resolve<IGameStateProvider>();
    }
    public override void InstallBindings()
    {
        Container.Bind<BoardService>().AsSingle();
        var gameState = _gameStateProvider.PuzzleState;
        var gridService = Container.Resolve<GridService>();
        var boardService = Container.Resolve<BoardService>();
        var puzzleService = new PuzzleService(gameState.Puzzles, _gameStateProvider, gridService, boardService);
        //// TEST

        //puzzleService.CreatePuzzle();
        //puzzleService.CreatePuzzle();
        //puzzleService.CreatePuzzle();
        //puzzleService.CreatePuzzle();



        ////
        Container.Bind<IPuzzleGenerator>().To<DefaultPuzzleGenerator>().AsSingle().NonLazy();
        var worldGameplayRootViewModel = new WorldGameplayRootViewModel(puzzleService);
        Container.Bind<PuzzleService>().FromInstance(puzzleService).AsSingle().NonLazy();

        Container.Bind<PuzzlesGenerator>().AsSingle().NonLazy();
        Container.Bind<InputService>().AsSingle().NonLazy();
        Container.Bind<WorldGameplayRootViewModel>().FromInstance(worldGameplayRootViewModel).AsSingle().NonLazy();
    }
}
