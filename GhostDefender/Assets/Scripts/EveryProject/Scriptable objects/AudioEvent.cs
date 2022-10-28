using UnityEngine;

namespace Scriptable_objects
{
    [System.Serializable]
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource audioSource);
    }
}
