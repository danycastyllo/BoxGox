using UnityEngine;
using UnityEngine.SceneManagement;
using FrameLord.StateManager;

public class StateMaze : State
{
    protected override void OnEnterState()
    {
        SceneManager.LoadScene("Maze");
    }
}
