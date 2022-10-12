// Unity Framework
using UnityEngine;
using UnityEngine.SceneManagement;

// FrameLord 
using FrameLord.Core;
using FrameLord.EventDispatcher;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [HideInInspector]
    // Player score
    public int score;

    // Clear hi-score on editor in each run?
    public bool clearHighscoreOnEditor = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor && clearHighscoreOnEditor)
        {
            HighscoreManager.Reset();
        }

        HighscoreManager.ReadHighscoreFromStorage();

        DontDestroyOnLoad(this);
    }

    public void NotifyPlayerDeath()
    {
        GameEventDispatcher.Instance.Dispatch(this, new EvnPlayerDied());
        HighscoreManager.WriteScoreToStorage();
    }

    public void NotifyEnemyDeath(int points)
    {
        GameEventDispatcher.Instance.Dispatch(this, new EvnScoreUpdate(points));
    }

    public void Reset()
    {
        score = 0;
    }
}