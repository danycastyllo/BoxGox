using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public static int highscore;
    private const string HighscoreKey = "highscore";

    /// <summary>
    /// Read the hi-score from storage
    /// </summary>
    public static void ReadHighscoreFromStorage()
    {
        highscore = PlayerPrefs.GetInt(HighscoreKey, 0);
    }


    /// <summary>
    /// Write the hi-score to the storage
    /// </summary>
    public static void WriteScoreToStorage()
    {
        PlayerPrefs.SetInt(HighscoreKey, highscore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Reset the hi-score
    /// </summary>
    public static void Reset()
    {
        PlayerPrefs.SetInt(HighscoreKey, 0);
        PlayerPrefs.Save();
    }
}
