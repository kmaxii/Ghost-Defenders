using Interfaces;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Events;

namespace MonoBehaviours
{
    public class GameEventListener : MonoBehaviour, IEventListenerInterface
    {
        public GameEvent @event;
        public UnityEvent response;

        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            if (enabled)
                response.Invoke();
        }
    }
}
