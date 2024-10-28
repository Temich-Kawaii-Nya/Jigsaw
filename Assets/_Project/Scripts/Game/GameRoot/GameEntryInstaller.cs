using Zenject;

public class GameEntryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameStateProvider>().To<PlayerPrefsGameStateProvider>().AsSingle().NonLazy();
    }
}
