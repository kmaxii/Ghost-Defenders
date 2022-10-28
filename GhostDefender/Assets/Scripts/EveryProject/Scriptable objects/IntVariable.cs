using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/data/int")]
    public class IntVariable : DataCarrier
    {
        [SerializeField] private int value;
        public GameEvent raiseOnValueChanged;

        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                if (raiseOnValueChanged)
                    raiseOnValueChanged.Raise();
            }
        }
        
       
    }
}
