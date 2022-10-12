// Unity Framework
using UnityEngine;
using FrameLord.StateManager;

namespace FrameLord.StateManager
{
    public class ConTimer : StateConnection
    {
        public float time;

        private float _accumTime;
        
        protected override void OnCheckCondition()
        {
            _accumTime += Time.deltaTime;
            _isFinished = (_accumTime >= time);
        }
        
        protected override void OnReset()
        {
            _accumTime = 0f;
        }
    }
}