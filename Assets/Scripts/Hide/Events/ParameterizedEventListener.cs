using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hide.Events
{
    [Serializable]
    public class ParameterizedEventListener<T> : MonoBehaviour
    {
        public ParameterizedGameEvent<T> gameEvent;

        public ParameterizedResponse response;

        [Serializable]
        public class ParameterizedResponse : UnityEvent<T> { }

        public void OnEventRaised(T arg)
        {
            response.Invoke(arg);
        }

        public void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }
    }
}