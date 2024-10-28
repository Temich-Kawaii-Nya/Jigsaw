public class MainMenuEnterParams
{
    public int SoftCurrency { get; }
    public int HardCurrency { get; }
    public int Score { get; }
    public int Time { get; }
    public MainMenuEnterParams(int softCurrency, int hardCurrency, int score, int time)
    {
        SoftCurrency = softCurrency;
        HardCurrency = hardCurrency;
        Score = score;
        Time = time;
    }
}
