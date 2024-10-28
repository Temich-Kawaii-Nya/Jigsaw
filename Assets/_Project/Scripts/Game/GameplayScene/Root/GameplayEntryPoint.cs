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
        var uiScene = Instantiate(_gameplayRootBinderPrefub);
        UIRoot.AttachScreenUI(uiScene.gameObject);

        var exitSceneSignalSubj = new Subject<Unit>();

        uiScene.Bind(exitSceneSignalSubj);

        _container.BindInterfacesAndSelfTo<DefaultImageLoader>().AsSingle().NonLazy();
        
        var adapter = new GameplayEnterParamsAdapter(enterParams, _container.Resolve<IImageLoader>());

        _container.BindInstance<GameplayEnterParamsAdapter>(adapter).AsSingle();
         
        var grid = new Grid(adapter.GridSize, adapter.CellSize);
        var gridProxy = new GridProxy(grid);

        _container.Bind<GridService>().AsSingle().WithArguments(_worldGrid, gridProxy);

        var mainMenuEnterParams = new MainMenuEnterParams(1, 2, 3, 4);
        var exitParams = new GameplayExitParams(mainMenuEnterParams);

        var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);

        GameplayInstaller.Install(_container);


        var puzzleGenerator = _container.Resolve<PuzzlesGenerator>();

        puzzleGenerator.Generate();

        var _generator = new DefaultPuzzleGenerator(
            enterParams.ImageId, 
            enterParams.DecoratorId, 
            enterParams.Size);

        _generator.Generate();

        _worldGameplayRootBinderPrefub = Instantiate(_worldGameplayRootBinderPrefub);
        _worldGameplayRootBinderPrefub.Bind(_container.Resolve<WorldGameplayRootViewModel>(), _generator);

        return exitToMainMenuSceneSignal;
    }
}
