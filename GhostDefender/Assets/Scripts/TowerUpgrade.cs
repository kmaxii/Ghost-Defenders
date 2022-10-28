using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/TowerUpgrades")]
    public class TowerUpgrade : ScriptableObject
    {
        [NonReorderable] 
        public StatsUpgrade[] path1 = new StatsUpgrade[3];
        [NonReorderable] 
        public StatsUpgrade[] path2 = new StatsUpgrade[3];
    }
}
