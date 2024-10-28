public class MainMenuExitParams 
{
    public GameplayEnterParams GameplayEnterParams { get; }
    public MainMenuExitParams(GameplayEnterParams gameplayEnterParams)
    {
        GameplayEnterParams = gameplayEnterParams;  
    }
}
