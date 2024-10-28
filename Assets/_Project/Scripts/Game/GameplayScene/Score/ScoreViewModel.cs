using R3;
using UnityEngine;

public class ScoreViewModel : IEventReceiver<ScoreIncreaseEvent>
{
    private readonly ScoreModelProxy _model;
    private readonly EventBus _eventBus;
    private readonly ScoreService _scoreService;

    public ReadOnlyReactiveProperty<int> Score { get; }
    public ScoreViewModel(ScoreModelProxy scoreModel, EventBus eventBus, ScoreService scoreService)
    {
        Debug.Log("DSFS");
        _model = scoreModel;
        _eventBus = eventBus;
        _scoreService = scoreService;   
        Score = scoreModel.Score;
        _eventBus.Register<ScoreIncreaseEvent>(this);
    }

    public void OnEvent(ScoreIncreaseEvent eventMessage)
    {
        _scoreService.IncreaseScore(eventMessage.score);
    }

}
