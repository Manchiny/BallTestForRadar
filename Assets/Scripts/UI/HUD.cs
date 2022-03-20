using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _scoresText;
    [SerializeField] private TextMeshProUGUI _speedText;

    public void ChangeScoresText(int value)
    {
        _scoresText.text = $"Scores: {value}";
    }
    public void ChangeLivesText(int value)
    {
        _livesText.text = $"Lives: {value}";
    }
    public void ChangeSpeedText(float value)
    {
        var speed = Math.Round(value, 2).ToString();
        _speedText.text = $"Speed: {speed}";
    }
}
