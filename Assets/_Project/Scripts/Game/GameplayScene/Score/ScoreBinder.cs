using TMPro;
using R3;
using UnityEngine;

public class ScoreBinder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private ScoreViewModel _scoreViewModel;
    public void Bind(ScoreViewModel viewModel)
    {
        _scoreViewModel = viewModel;
        viewModel.Score.Subscribe(e =>
        {
            UpdateText(e);
        });
    }
    public void UpdateText(int newAmount)
    {
        Debug.Log("ds");
        _scoreText.text = newAmount.ToString();
    }
}
