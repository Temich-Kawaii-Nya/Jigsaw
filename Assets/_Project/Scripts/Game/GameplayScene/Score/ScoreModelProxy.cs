using R3;

public class ScoreModelProxy
{
    public ReactiveProperty<int> Score { get; }
    public ReactiveProperty<float> ScoreModifier { get; }
    private readonly ScoreModel _origin;
    public ScoreModelProxy(ScoreModel origin)
    {
        _origin = origin;

        Score = new(origin.score);

        Score.Subscribe(e =>
        {
            if (e < 0)
                e = 0;
            origin.score = e;
        });

        ScoreModifier = new(origin.scoreModifier);
        Score.Subscribe(e =>
        {
            origin.scoreModifier = e;
        });

    }
}
