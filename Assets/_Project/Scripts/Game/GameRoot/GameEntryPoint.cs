using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using R3;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;

    private Coroutines _coroutines;

    private UIRootView _uiRoot;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutoStartGame()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _instance = new GameEntryPoint();
        _instance.RunGame();
    }
    private GameEntryPoint()
    {
        _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        Object.DontDestroyOnLoad(_coroutines.gameObject);

        var prefubUIRoot = Resources.Load<UIRootView>("UIRoot");
        _uiRoot = Object.Instantiate(prefubUIRoot); 
        Object.DontDestroyOnLoad(_uiRoot.gameObject);

    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;

            var gamePlayEneterParams = new GameplayEnterParams(2, 1, new Vector2Int(3, 4), false);
        if (sceneName == Scenes.GAMEPLAY)
        {
            _coroutines.StartCoroutine(LoadAndStartGameplay(gamePlayEneterParams));
            return;
        }
        if (sceneName == Scenes.MAIN_MENU)
        {
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
            return;
        }
        if (sceneName != Scenes.BOOT)
        {
            return;
        }
#endif
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
    }
    private IEnumerator LoadAndStartGameplay(GameplayEnterParams enterParams)
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAMEPLAY);
        
        //
        yield return new WaitForSeconds(0.5f);
        //

        var isGameStateLoaded = false;
        var Container = ProjectContext.Instance.Container;        
        Container.Resolve<IGameStateProvider>().LoadPuzzleState().Subscribe(_ => isGameStateLoaded = true);
        yield return new WaitUntil(() => isGameStateLoaded);
        
        var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        sceneEntryPoint.Run(_uiRoot, enterParams).Subscribe(gameplayExitParams => 
        {
            _coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParams.MainMenuEnterParams));
        });

        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.MAIN_MENU);
        yield return new WaitForSeconds(1f);

        var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
        sceneEntryPoint.Run(_uiRoot, enterParams).Subscribe(mainMenuExitParams => 
        {
            _coroutines.StartCoroutine(LoadAndStartGameplay(mainMenuExitParams.GameplayEnterParams));
        });


        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

}
