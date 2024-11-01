using R3;
using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private UIGameplayRootBinder _gameplayRootBinderPrefub;
    [SerializeField] private WorldGameplayRootBinder _worldGameplayRootBinderPrefub;
    [SerializeField] private UnityEngine.Grid _worldGrid;
    [Inject] private DiContainer _container;
    public Observable<GameplayExitParams> Run(UIRootView UIRoot, GameplayEnterParams enterParams)
    {
        _container.BindInterfacesAndSelfTo<DefaultImageLoader>().AsSingle().NonLazy();
        var adapter = new GameplayEnterParamsAdapter(enterParams, _container.Resolve<IImageLoader>());
        _container.BindInstance<GameplayEnterParamsAdapter>(adapter).AsSingle();
        var grid = new Grid(adapter.GridSize, adapter.CellSize);
        var gridProxy = new GridProxy(grid);
        _container.Bind<GridService>().AsSingle().WithArguments(_worldGrid, gridProxy);
        GameplayInstaller.Install(_container);
        var uiScene = _container.InstantiatePrefabForComponent<UIGameplayRootBinder>(_gameplayRootBinderPrefub);
        
        UIRoot.AttachScreenUI(uiScene.gameObject);

        var exitSceneSignalSubj = new Subject<Unit>();

        uiScene.Bind(exitSceneSignalSubj);

        var mainMenuEnterParams = new MainMenuEnterParams(1, 2, 3, 4);
        var exitParams = new GameplayExitParams(mainMenuEnterParams);

        var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);

        var puzzleGenerator = _container.Resolve<PuzzlesGenerator>();

        puzzleGenerator.Generate();

        var _generator = new DefaultPuzzleGenerator(
            enterParams.ImageId, 
            enterParams.DecoratorId, 
            enterParams.Size);

        _generator.Generate();

        _worldGameplayRootBinderPrefub = Instantiate(_worldGameplayRootBinderPrefub);
        _worldGameplayRootBinderPrefub.Bind(_container.Resolve<WorldGameplayRootViewModel>(), _generator);

        var boardFactory = _container.Resolve<BoardFactory>();
       
        boardFactory.CreateBackground();
        boardFactory.CreateBackgroundBoard();


        // TEST
        _container.Resolve<PuzzleService>().ShufflePuzzles(); //TODo
        //


        return exitToMainMenuSceneSignal;
    }
}
