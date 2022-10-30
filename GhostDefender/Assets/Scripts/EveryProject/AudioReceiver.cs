using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

public class AudioReceiver : MonoBehaviour
{

    private readonly List<AudioSource> _audioSources = new List<AudioSource>();

    private readonly Dictionary<AudioEvent, List<AudioSource>> _audioEventsPlaying = new Dictionary<AudioEvent, List<AudioSource>>();


    // Update is called once per frame
    void Update()
    {
        if (_audioSources.Count > 10)
        {
            AudioSource audioSource = _audioSources[10];
            if (!audioSource.isPlaying)
            {
                Destroy(audioSource);
                _audioSources.RemoveAt(10);
            }
        }

        foreach (var list in _audioEventsPlaying.Values)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                AudioSource audio = list[i];
             
                if (audio.isPlaying)
                    continue;
                
                list.RemoveAt(i);
            }
        }
    }


    public void OnReceiveAudio(AudioEvent audioEvent)
    {
        if (!_audioEventsPlaying.ContainsKey(audioEvent))
            _audioEventsPlaying.Add(audioEvent, new List<AudioSource>());
        
        if (_audioEventsPlaying[audioEvent].Count > 10)
            return;
        
        //Searches for a not playing audio source
        for (int i = 0; i < _audioSources.Count; i++)
        {
            AudioSource audioSource = _audioSources[i];
            if (audioSource.isPlaying)
                continue;
            //Found unused audio source. Playing using that
            audioEvent.Play(audioSource);
            AddToPlaying(audioEvent, audioSource);

            return;
        }
        
        //No unused audio source was found. So adding a new one.
        _audioSources.Add(gameObject.AddComponent<AudioSource>());
        audioEvent.Play(_audioSources[^1]);
    }

    private void AddToPlaying(AudioEvent audioEvent, AudioSource audioSource)
    {
     _audioEventsPlaying[audioEvent].Add(audioSource);
        
    }
}
