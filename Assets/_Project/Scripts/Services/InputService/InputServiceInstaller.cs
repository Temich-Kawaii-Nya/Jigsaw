using Zenject;

public class InputServiceInstaller : MonoInstaller<InputServiceInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<InputService>().AsSingle().NonLazy();
    }
}
