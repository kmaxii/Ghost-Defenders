using System.Collections.Generic;
using MonoBehaviors;
using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/Event/GameEventWithData")]
    public class GameEventWithData : ScriptableObject
    {
        private readonly List<GameEventListenerWithData> _listeners = new List<GameEventListenerWithData>();
        
    
            public void Raise(DataCarrier data){
                for (int i = _listeners.Count - 1; i >= 0; i--)
                {
                    _listeners[i].OnEventRaised(data);
                }
            }

            public void RegisterListener(GameEventListenerWithData listener)
            {
                _listeners.Add(listener);
            }

            public void UnregisterListener(GameEventListenerWithData listener)
            {
                _listeners.Remove(listener);
            }
        }
}
