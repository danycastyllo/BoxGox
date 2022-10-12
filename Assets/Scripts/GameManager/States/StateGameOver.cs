using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.StateManager;

public class StateGameOver : State
{
    protected override void OnEnterState()
    {
        Time.timeScale = 0f;
    }

    protected override void OnLeaveState()
    {
        Time.timeScale = 1f;
    }
}
