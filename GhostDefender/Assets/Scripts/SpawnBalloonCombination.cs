using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnBalloonCombination : IChildFinished
{
    private float _spawningDelay;

    private readonly IChildFinished _parent;
    private readonly Queue<WaveElementWithDelay> _inputCombo;
    private readonly Spawner _spawner;

    private int _sending;
    
    public SpawnBalloonCombination(WaveElementWithDelay waveElement, Spawner spawner, IChildFinished parent)
    {
        _spawner = spawner;
        _parent = parent;

        if (waveElement.waveElement is BalloonCombination combination)
        {
            
            foreach (var waveElementWithDelay in combination.combination)
            {
                waveElementWithDelay.afterPreviousFinished = true;
            }
            
            _inputCombo = new Queue<WaveElementWithDelay>(combination.combination);
            return;
        }

        _inputCombo = new Queue<WaveElementWithDelay>();
        _inputCombo.Enqueue(waveElement);
    }


    public void Start()
    {
        _spawner.StartCoroutine(RunThroughQueue());
    }

    public void Continue(SpawnBalloonCombination triggeredBy)
    {
        Start();
    }


    private IEnumerator RunThroughQueue()
    {
        while (_inputCombo.TryDequeue(out var waveElement))
        {
            //If the next element in the Queue should wait until the previous element to finish & it has not waited already

            yield return new WaitForSeconds(waveElement.StartDelay);

            
            Assert.IsTrue(waveElement.waveElement is Spawnable);

            _spawningDelay = (waveElement.amount * waveElement.delayBetween);


            for (int i = 0; i < waveElement.amount; i++)
            {
                Spawn(waveElement);

                if (i + 1 != waveElement.amount)
                    yield return new WaitForSeconds(waveElement.delayBetween);
            }
        }

        Finished();
    }


    private void Finished()
    {
        if (_parent != null && _sending == 0)
        {
            _parent.Continue(this);
        }
    }


    private void Spawn(WaveElementWithDelay waveElementWithDelay)
    {
        WaveElement waveElement = waveElementWithDelay.waveElement;
        Assert.IsTrue(waveElement is Spawnable);

        Spawnable balloon = (Spawnable) waveElement;

        SpawnNewBalloon(balloon, waveElementWithDelay.isCamo, waveElementWithDelay.isRegen);
    }


    private void SpawnNewBalloon(Spawnable balloon, bool isCamo, bool isRegen)
    {
        _spawner.InstantiateBalloon(balloon, isCamo, isRegen);
    }
}