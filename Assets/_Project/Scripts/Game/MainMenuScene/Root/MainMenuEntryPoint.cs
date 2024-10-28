using R3;
using UnityEngine;

public class MainMenuEntryPoint : MonoBehaviour
{

    [SerializeField] private UIMainMenuRootBinder _mainMenuRootBinderPrefub;

    public Observable<MainMenuExitParams> Run(UIRootView UIRoot, MainMenuEnterParams mainMenuEnterParams)
    {
        var uiScene = Instantiate(_mainMenuRootBinderPrefub);
        UIRoot.AttachScreenUI(uiScene.gameObject);

        var exitSceneSignalSubj = new Subject<Unit>();

        uiScene.Bind(exitSceneSignalSubj);

        var gamePlayEnterParams = new GameplayEnterParams(1, 1, new Vector2Int(4, 4), false);
        var exitParams = new MainMenuExitParams(gamePlayEnterParams);

        var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);

        return exitToMainMenuSceneSignal;
    }
}
