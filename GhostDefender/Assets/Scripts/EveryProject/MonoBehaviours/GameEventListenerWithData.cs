using Scriptable_objects;
using UnityEngine;
using UnityEngine.Events;

namespace MonoBehaviors
{
    public class GameEventListenerWithData : MonoBehaviour
    {
        public GameEventWithData @event;
        public DataEventEvent response;

        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised(DataCarrier data)
        {
            if (enabled)
                response.Invoke(data);
        }
    }
}
