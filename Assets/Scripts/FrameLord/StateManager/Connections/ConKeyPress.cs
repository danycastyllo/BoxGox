using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.StateManager;

namespace FrameLord.StateManager
{
    public class ConKeyPress : StateConnection
    {
        public KeyCode keyToPress;
        
        protected override void OnCheckCondition()
        {
            _isFinished = Input.GetKeyDown(keyToPress);
        }
    }
}