using System;
using System.Collections;
using System.Collections.Generic;

// Unity Framework
using UnityEngine;
using UnityEngine.UI;

// FrameLord
using FrameLord.EventDispatcher;

public class ScoreUIManager : MonoBehaviour
{
    // Player score text reference
    public Text playerScore;

    // Hi score text reference
    public Text highscore;

    /// <summary>
    /// Unity Start Method
    /// </summary>
    void Start()
    {
        playerScore.text = $"{GameManager.Instance.score}";
        highscore.text = $"{HighscoreManager.highscore}";

        GameEventDispatcher.Instance.AddListener(EvnScoreUpdate.Name, OnScoreUpdate);
    }

    private void OnDestroy()
    {
        GameEventDispatcher.Instance.RemoveListener(EvnScoreUpdate.Name, OnScoreUpdate);
    }

    public void AddPlayerScore(int valueToAdd)
    {
        GameManager.Instance.score += valueToAdd;
        playerScore.text = $"{GameManager.Instance.score}";

        if (GameManager.Instance.score > HighscoreManager.highscore)
        {
            HighscoreManager.highscore = GameManager.Instance.score;
            highscore.text = $"{HighscoreManager.highscore}";
        }
    }

    private void OnScoreUpdate(System.Object sender, GameEvent e)
    {
        AddPlayerScore(((EvnScoreUpdate)e).scoreIncrementValue);
    }
}
