using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scriptable_objects
{
    [System.Serializable]
    public class StatsUpgrade
    {

        [Header("Description")]
        [Tooltip("Name the upgrade shows up as")]
        public String upgradeName;
        [Tooltip("Explanation of the upgrade")]
        public String description;
        [Tooltip("Image to show for the upgrade")]
        public Sprite upgradeImage;
        
        [Header("Money")]
        [Tooltip("The price of this upgrade")]
        public int price;
        
        [Header("Stat changes")]
        [Tooltip("The attack speed that should be added with this upgrade")]
        public float attackCooldown;
        [Tooltip("The damage that should be added with this upgrade")]
        public int damage;
        [Tooltip("The amount of balloons this can destroy on one shot")]
        public int pierce;
        [Tooltip("The projectile speed that should be added with this upgrade")]
        public int projectileSpeed;
        [Tooltip("If this tower can detect camos after this upgrade")]
        public bool canDetectCamosAfter;
        
        [Header("Other")]
        [Tooltip("A new sprite, if this upgrade changes this towers look")]
        public Sprite newSprite;
    }
}
