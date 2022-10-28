using UnityEngine;

namespace Scriptable_objects
{
  [CreateAssetMenu(menuName = "Custom/data/float")]
  public class FloatVariable : DataCarrier
  {
    public float value;
  }
}
