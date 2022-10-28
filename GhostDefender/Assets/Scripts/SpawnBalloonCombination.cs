using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Scriptable_objects;
using UnityEngine;

public class SpawnBalloonCombination : IChildFinished
{
    private SpawnBalloonCombination _latestChild;
    private bool _isWaiting;
    private float _spawningDelay;

    private readonly IChildFinished _parent;
    private readonly Queue<WaveElementWithDelay> _inputCombo;
    private readonly Spawner _spawner;

    private int _sending;

    private SpawnBalloonCombination(BalloonCombination combination, Spawner spawner, IChildFinished parent)
    {
        _spawner = spawner;
        _inputCombo = new Queue<WaveElementWithDelay>(combination.combination);
        _parent = parent;
    }

    public SpawnBalloonCombination(WaveElementWithDelay waveElement, Spawner spawner, IChildFinished parent)
    {
        _spawner = spawner;
        if (waveElement.waveElement is BalloonCombination combination)
        {
            _inputCombo = new Queue<WaveElementWithDelay>(combination.combination);
        }
        else if (waveElement.waveElement is Spawnable balloon)
        {
            _inputCombo = new Queue<WaveElementWithDelay>();
            _inputCombo.Enqueue(waveElement);
        }

        _parent = parent;
    }


    public void Start()
    {
        _spawner.StartCoroutine(RunThroughQueue());
    }

    public void Continue(SpawnBalloonCombination triggeredBy)
    {
        if (_latestChild == null || triggeredBy != _latestChild) return;
        _latestChild = null;
        Start();
    }


    private IEnumerator RunThroughQueue()
    {
        while (_inputCombo.TryPeek(out var waveElement))
        {
            //If the next element in the Queue should wait until the previous element to finish & it has not waited already
            if (waveElement.afterPreviousFinished && !_isWaiting)
            {
                //The last thing that was spawned where simple balloons. So wait until they are finished spawning
                if (_latestChild == null)
                {
                    yield return new WaitForSeconds(_spawningDelay);
                }
                //The last thing that was spawned was a combination, so wait until that is finished.
                else
                {
                    _isWaiting = true;
                    yield break;
                }
            }

            //Set isWaiting to false so that the if statement above can trigger next time something is marked as start on finish
            _isWaiting = false;

            waveElement = _inputCombo.Dequeue();

            yield return new WaitForSeconds(waveElement.StartDelay);

            _spawner.StartCoroutine(Send(waveElement));
           
            _sending++;
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
        
        //If this element is a combination of balloons
        if (waveElement is BalloonCombination combination)
        {
            SpawnBalloonCombination newCombo = new SpawnBalloonCombination(combination, _spawner, this);
            _latestChild = newCombo;
            newCombo.Start();
        }
        else if (waveElement is Spawnable balloon)
        {
            _latestChild = null;
            SpawnNewBalloon(balloon, waveElementWithDelay.isCamo, waveElementWithDelay.isRegen);
        }
        else
        {
            Debug.LogError("Wrong kind of data sent.", _spawner);
        }
    }

    private IEnumerator Send(WaveElementWithDelay waveElementWithDelay)
    {
        if (waveElementWithDelay.waveElement is Balloon)
        {
            _spawningDelay = (waveElementWithDelay.amount * waveElementWithDelay.delayBetween) + waveElementWithDelay.StartDelay;
        }
        

        for (int i = 0; i < waveElementWithDelay.amount; i++)
        {
            Spawn(waveElementWithDelay);
            
            if (i + 1 != waveElementWithDelay.amount)
                yield return new WaitForSeconds(waveElementWithDelay.delayBetween);
        }

        _sending--;
        Finished();
    }

    private void SpawnNewBalloon(Spawnable balloon, bool isCamo, bool isRegen)
    {
        _spawner.InstantiateBalloon(balloon, isCamo, isRegen);
    }
}