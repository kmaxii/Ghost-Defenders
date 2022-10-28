using System.Collections.Generic;
using Interfaces;
using MonoBehaviors;
using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/Event/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<IEventListenerInterface> _listeners = new List<IEventListenerInterface>();
    
        public void Raise(){
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(IEventListenerInterface listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(IEventListenerInterface listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
    }
}
