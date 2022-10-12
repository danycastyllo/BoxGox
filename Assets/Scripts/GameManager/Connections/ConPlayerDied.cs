using System.Collections;
using System.Collections.Generic;
using FrameLord.EventDispatcher;
using UnityEngine;
using UnityEngine.EventSystems;

// FrameLord
using FrameLord.StateManager;

public class ConPlayerDied : StateConnection
{
    protected override void OnInit()
    {
        GameEventDispatcher.Instance.AddListener(EvnPlayerDied.Name, OnPlayerDied);
    }

    void OnPlayerDied(System.Object sender, GameEvent e)
    {
        _isFinished = true;
    }
}
