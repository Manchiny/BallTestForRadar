using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HUD _hud;
    [SerializeField] private GameOverWindow _gameOverWindowPrefab;
    [SerializeField] private RectTransform _windowHolder;

    public void Init(Ball ball)
    {
        Game.Instance.OnScoresChanged += _hud.ChangeScoresText;
        Game.Instance.OnLivesChanged += _hud.ChangeLivesText;
        ball.OnCurrenSpeedChanged += _hud.ChangeSpeedText;

        Game.Instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        var window = Instantiate(_gameOverWindowPrefab, _windowHolder);
        window.Init(Game.Instance.Scores, Game.Instance.RestartGame);
    }
}
