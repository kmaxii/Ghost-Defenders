using System;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

namespace Towers
{
    public delegate void BulletFire(int i, int y, int z, float x, IntVariable balloonsPopped);  

    public class ShootingTower : Tower
    {
        private List<PathFollower> _targetsInRange;

        private float _time;
        private readonly float _updateDelay = 0.5f;

        [HideInInspector] private GameObject _furthestInRange;

        public GameObject furthestInRange
        {
            get => _furthestInRange;
            private set => _furthestInRange = value;
        }
        
        public event BulletFire ShootEvent; // event

        private float _timer;
        

        [SerializeField] private FloatVariable defaultRange;

        private bool _lookAtTarget;

        [SerializeField] private String animationOnFire;

        [SerializeField] private float _extraRangeOffset = 1f;
        // Start is called before the first frame update
        void Start()
        {
            _targetsInRange = new List<PathFollower>();
            //Divide local scale by 2 as local scale is diameter and not radius
            _towerCollider.radius = defaultRange.value / (transform.localScale.x / 2f);
        }

     
        private void Update()
        {
           
            if (furthestInRange && !furthestInRange.activeSelf)
            {
                _targetsInRange.Remove(furthestInRange.GetComponent<PathFollower>());
                furthestInRange = null;
            }
            
            _time += Time.deltaTime;
            if (_time >= _updateDelay)
            {
                UpdateTarget();
                _time = 0;
            }
            
            _timer += Time.deltaTime;
            
            
            if (!furthestInRange)
                return;

 
            if (_lookAtTarget)
            {
                LookAtTarget();
            }
            
            
            if (_timer < shootingCooldown) return;

            
            PrepareShoot();
        }

        private void LookAtTarget()
        {
            Vector3 diff =  transform.position - furthestInRange.transform.position;
            diff.Normalize();
 
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.tag.Equals("Enemy")) return;
        
            _targetsInRange.Add(other.gameObject.GetComponent<PathFollower>());
            
            //If there is no current target, get one
            if(!furthestInRange)
                furthestInRange = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.tag.Equals("Enemy")) return;


            _targetsInRange.Remove(other.gameObject.GetComponent<PathFollower>());
            if (furthestInRange == other.gameObject)
            {
                UpdateTarget();
            }

        }

        private void UpdateTarget()
        {
            var furthest = TryGetFurthestInRange(out var inRange) ? inRange : null;
            if (furthest != furthestInRange)
            {
                furthestInRange = furthest;
            }
        }

        private bool TryGetFurthestInRange(out GameObject furthest)
        {
            furthest = null;
            float furthestTarget = 0f;
            bool found = _targetsInRange.Count > 0;
            for (int i = _targetsInRange.Count - 1; i >= 0; i--)
            {
                PathFollower pathFollower = _targetsInRange[i];
                if (pathFollower == null)
                {
                    _targetsInRange.RemoveAt(i);
                    continue;
                }
                if (pathFollower.distanceTraveled > furthestTarget)
                {
                    furthest = pathFollower.gameObject;
                    furthestTarget = pathFollower.distanceTraveled;
                }
            }

            if (furthest == null)
            {
                found = false;
            }

            return found;
        }
        
        private void PrepareShoot()
        {
            if (!furthestInRange.activeSelf)
            {
                _targetsInRange.Remove(furthestInRange.GetComponent<PathFollower>());
                furthestInRange = null;

                return;
            }
            _timer = 0;

            _lookAtTarget = true;

            _animator.SetTrigger(animationOnFire);
            Invoke(nameof(Fire), 0.1f);
        }

        private void Fire()
        {
            if(!furthestInRange) return;

            LookAtTarget();
            ShootEvent?.Invoke(damage, projectileSpeed, pierce, (_towerCollider.radius * transform.localScale.x) + _extraRangeOffset, balloonsPopped);
            _lookAtTarget = false;
        }
        
        public void SetFiringAnimation(string animationName)
        {
            animationOnFire = animationName;
        }
        
    }
}
