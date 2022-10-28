using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/SpriteAnimation")]
    public class SpriteAnimation : ScriptableObject
    {
        public float delayBetweenChanges;
        public float size;
        public Sprite[] sprites;
    }
}
