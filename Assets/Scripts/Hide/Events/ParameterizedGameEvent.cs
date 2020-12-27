using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hide.Events
{
    [Serializable]
    public class ParameterizedGameEvent<T> : ScriptableObject
    {
        private readonly List<ParameterizedEventListener<T>> _eventListeners =
            new List<ParameterizedEventListener<T>>();

        public void Raise(T arg)
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
            {
                _eventListeners[i].OnEventRaised(arg);
            }
        }

        public void RegisterListener(ParameterizedEventListener<T> listener)
        {
            if (!_eventListeners.Contains(listener))
            {
                _eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(ParameterizedEventListener<T> listener)
        {
            if (_eventListeners.Contains(listener))
            {
                _eventListeners.Remove(listener);
            }
        }
    }
}