using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/ShopElement")]
    public class ShopElement : ScriptableObject
    {
        public Sprite shopIcon;
        public GameObject prefabToSpawn;
        public int cost;
        public StringList cantSpawnOn;
        public FloatVariable defaultRange;

    }
}
