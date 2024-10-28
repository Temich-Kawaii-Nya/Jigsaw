using Zenject;
using UnityEngine;
public class ScoreInstaller : MonoInstaller
{
    [SerializeField] private ScoreBinder scoreBinder;
    public override void InstallBindings()
    {
        var model = new ScoreModel();
        var proxy = new ScoreModelProxy(model);
        var score = new ScoreService(proxy);
        var eventBus = Container.Resolve<EventBus>();
        var vm = new ScoreViewModel(proxy, eventBus, score);
        scoreBinder.Bind(vm);
    }
}
