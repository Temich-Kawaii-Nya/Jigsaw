using Zenject;

public class ScoreService : IInitializable
{
    private readonly ScoreModelProxy _scoreModel;
    private float _prevScoreModifier;

    public ScoreService(
        ScoreModelProxy model)
    {
        _scoreModel = model;
    }

    public void IncreaseScore(int amount)
    {
        _scoreModel.Score.Value += amount;
    }
    public void DecreaseScore(int amount)
    {
        _scoreModel.Score.Value -= amount;
    }
    public void SetScoreModifier(float modifier)
    {
        _scoreModel.ScoreModifier.Value = modifier;
    }
    public void SetDefaultScoreModifier()
    {
        _scoreModel.ScoreModifier.Value = _prevScoreModifier;
    }
    public void Initialize()
    {
        _prevScoreModifier = _scoreModel.ScoreModifier.Value;
    }
}
