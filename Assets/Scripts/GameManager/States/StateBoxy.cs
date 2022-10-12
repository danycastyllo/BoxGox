using UnityEngine;
using UnityEngine.SceneManagement;
using FrameLord.StateManager;

public class StateBoxy : State
{
    protected override void OnEnterState()
    {
        SceneManager.LoadScene("Boxy");
    }
}
