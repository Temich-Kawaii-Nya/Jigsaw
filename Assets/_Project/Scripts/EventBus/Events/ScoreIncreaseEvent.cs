public struct ScoreIncreaseEvent : IEvent
{
    public int score;

    public ScoreIncreaseEvent(int amount)
    {
        score = amount;
    }
}
public struct ScoreDecreaseEvent : IEvent
{
    public int score;

    public ScoreDecreaseEvent(int amount) 
    { 
        score = amount; 
    }
}
