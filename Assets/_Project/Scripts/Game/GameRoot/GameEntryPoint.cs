using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (sceneName == Scenes.GAMEPLAY)
        {
            _coroutines.StartCoroutine(LoadAndStartGameplay());
            return;
        }
        if (sceneName != Scenes.BOOT)
        {
            return;
        }
#endif
    }
    private IEnumerator LoadAndStartGameplay()
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAMEPLAY);
        yield return new WaitForSeconds(1f);

        var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        sceneEntryPoint.Run();

        _uiRoot.HideLoadingScreen();
    }
    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

}
