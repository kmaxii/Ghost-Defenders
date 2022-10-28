using System;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Events;

namespace Towers
{
    public abstract class Tower : MonoBehaviour
    {

        private Vector2Int _upgradeState = Vector2Int.zero;
        private Vector2Int _currentTexture = Vector2Int.zero;
        
        [Header("Must have")]
        [SerializeField] private TowerUpgrade towerUpgrade;

        [SerializeField] private ChangeSpriteEvent _spriteChanger;
        [SerializeField] private ReachRing _reachRing;
        [SerializeField] protected CircleCollider2D _towerCollider;
        [SerializeField] private IntVariable money;
        [SerializeField] private ShopElement shopElement;

        
        [Header("Stats")]
        [SerializeField] protected float shootingCooldown = 1;
        [SerializeField] protected int damage = 1;
        [SerializeField] protected int pierce = 1;
        [SerializeField] protected int projectileSpeed = 2000;
        private bool _canDetectCamos = false;
        public IntVariable balloonsPopped;

        [HideInInspector]
        public int _totalMoneySpentOnThisTower;

        [SerializeField] private RuntimeAnimatorController _controller;
        protected Animator _animator;


        public bool CanDetectCamos
        {
            get => _canDetectCamos; 
            set => _canDetectCamos = value;
        }
        
        [Header("On Upgrades")]
        [NonReorderable]
        [SerializeField] private UnityEvent[] onPath1Upgrade = new UnityEvent[4];
        [NonReorderable]
        [SerializeField] private UnityEvent[] onPath2Upgrade = new UnityEvent[4];

        private string _aiState = "First";

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _totalMoneySpentOnThisTower = shopElement.cost;
            balloonsPopped = ScriptableObject.CreateInstance<IntVariable>();
            balloonsPopped.Value = 0;
            balloonsPopped.raiseOnValueChanged = ScriptableObject.CreateInstance<GameEvent>();
            Debug.Log(balloonsPopped.Value);
        }



        public void OnTowerClick()
        {
            float radius = _towerCollider.radius;
            _reachRing.ShowRing(radius);
         //   upgradeMenu.gameObject.SetActive(true);
            DataForUpgradeScreen dataForUpgrade = new DataForUpgradeScreen(_spriteChanger.ActiveSprite, towerUpgrade.name, 21, _aiState, towerUpgrade, _upgradeState);
            UpgradeMenu.Show(dataForUpgrade, this, _reachRing);
        }

        public void UpgradeOne()
        {
            if (_upgradeState.x >= towerUpgrade.path1.Length) 
                return;
            
            Upgrade(towerUpgrade.path1[_upgradeState.x], new Vector2Int(_upgradeState.x + 1, 0));
            _upgradeState.x++;

            
            DataForUpgradeScreen dataForUpgrade = new DataForUpgradeScreen(_spriteChanger.ActiveSprite, towerUpgrade.name, 21, _aiState, towerUpgrade, _upgradeState);
            UpgradeMenu.Show(dataForUpgrade, this, _reachRing);
            
            float radius = _towerCollider.radius;
            _reachRing.ShowRing(radius);
        }
        
        public void UpgradeTwo()
        {
            if (_upgradeState.y >= towerUpgrade.path2.Length) 
                return;
            
            _upgradeState.y++;
            
            
            
            Upgrade(towerUpgrade.path2[_upgradeState.y - 1], new Vector2Int(0, _upgradeState.y));

            DataForUpgradeScreen dataForUpgrade = new DataForUpgradeScreen(_spriteChanger.ActiveSprite, towerUpgrade.name, 21, _aiState, towerUpgrade, _upgradeState);
            UpgradeMenu.Show(dataForUpgrade, this, _reachRing);
            
            float radius = _towerCollider.radius;
            _reachRing.ShowRing(radius);
        }


        private void Upgrade(StatsUpgrade upgrade, Vector2Int upgradeNumber)
        {

            money.Value -= upgrade.price;
            _totalMoneySpentOnThisTower += upgrade.price;
            shootingCooldown += upgrade.attackCooldown;
            damage += upgrade.damage;
            pierce += upgrade.pierce;
            projectileSpeed += upgrade.projectileSpeed;
            if (upgrade.canDetectCamosAfter)
                _canDetectCamos = true;
            Sprite newSprite = upgrade.newSprite;
         

            if (upgradeNumber.x > 0)
            {
                onPath1Upgrade[upgradeNumber.x - 1].Invoke();
                if (_currentTexture.y > upgradeNumber.x)
                    newSprite = null;
            }
            else
            {
                onPath2Upgrade[upgradeNumber.y - 1].Invoke();
                if (_currentTexture.x > upgradeNumber.y)
                    newSprite = null;
            }
            
            if (newSprite != null)
            
            {
                //Destroying and reading the animator to allow for sprite changes
                DestroyImmediate(_animator);
                _spriteChanger.ActiveSprite = newSprite;

                _animator = gameObject.AddComponent<Animator>();
                _animator.runtimeAnimatorController = _controller;
                
                _currentTexture = upgradeNumber;
            }
        }
    }
}
