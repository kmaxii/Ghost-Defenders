using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(menuName = "Custom/Waves/BalloonCombination")]
    public class BalloonCombination : WaveElement
    {
        public float defaultDelay;
        private float _defaultDelayChangeCheck;
        [NonReorderable]

        public List<WaveElementWithDelay> combination;

        private int _combinationSizeChangeCheck;

        private void OnValidate()
        {
            if (Math.Abs(_defaultDelayChangeCheck - defaultDelay) > 0.001f || _combinationSizeChangeCheck != combination.Count)
            {
               SetDefaultDelays();
            }

        }

        private void SetDefaultDelays()
        {
            for (int i = 0; i < combination.Count; i++)
            {
                combination[i].defaultDelay = defaultDelay;
            }

            _combinationSizeChangeCheck = combination.Count;
            _defaultDelayChangeCheck = defaultDelay;
        }
    }
    
    [Serializable]
    public class WaveElementWithDelay
    {
        [Header("Information for this to start")]
        [Tooltip("If no, the delay to start is counted from the time the last one started, if yes it it counted from when the last one finished spawning")]
        public bool afterPreviousFinished;
        [Tooltip("If a BalloonCombination is used the delay equals to distance between them. All will be spawned at once")]
        [SerializeField] private float delayToStart;
        [SerializeField] private bool useDefaultDelay;
        [HideInInspector] public float defaultDelay;

        public float StartDelay => useDefaultDelay ? defaultDelay : delayToStart;


        [Header("Spawn Multiple Option")]
        public int amount;
        [Tooltip("The delay between the spawning of multiple. If you are only spawning one then this is not needed.")]
        public float delayBetween;
        public WaveElement waveElement;

        [Tooltip("Explains itself, if the balloon is of regen type")]
        public bool isRegen;
        [Tooltip("Explains itself, if the balloon is of camo type")]
        public bool isCamo;

    }
    
}

