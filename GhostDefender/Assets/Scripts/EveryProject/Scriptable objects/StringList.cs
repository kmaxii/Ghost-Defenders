using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/data/stringList")]
    public class StringList : DataCarrier
    {
        [SerializeField] public string[] list;
    }
}