// Unity Framework
using UnityEngine;
using UnityEngine.SceneManagement;
using FrameLord.StateManager;

    public class StateMainMenu : State
    {
        protected override void OnEnterState()
        {
            SceneManager.LoadScene("MainMenu");
        }

        protected override void OnLeaveState()
        {
        GameManager.Instance.Reset();
        }
    }
