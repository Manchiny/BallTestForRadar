using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoresText;
    [SerializeField] private Button _restartGameButton;

    public void Init(int scores, Action restartGame)
    {
        _scoresText.text = $"Your scores: {scores}";

        _restartGameButton.onClick.RemoveAllListeners();
        _restartGameButton.onClick.AddListener(() => restartGame.Invoke());
        _restartGameButton.onClick.AddListener(OnButtonRestartClick);
    }

    private void OnButtonRestartClick()
    {
        Destroy(gameObject);
    }
}
