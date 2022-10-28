using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager _instance;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }



    private void PrivateFadeToSong(AudioClip audioClip, float timeToFadeIn, float timeToFadeOut)
    {
        StopAllCoroutines();
        
        StartCoroutine(FadeTrack(audioClip, timeToFadeOut, timeToFadeIn));
    }


    private IEnumerator FadeTrack(AudioClip audioClip, float timeToFadeOut, float timeToFadeIn)
    {
        float timeElapsed = 0f;

        while (timeElapsed < timeToFadeOut)
        {
            _audioSource.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFadeOut);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0f;

        _audioSource.clip = audioClip;
        _audioSource.Play();
        Debug.Log("Switched clip to "+ audioClip.name);
        
        
        while (timeElapsed < timeToFadeIn)
        {
            _audioSource.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFadeIn);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
       
        
    }
    
    
    public static void FadeToSong(AudioClip audioClip, float timeToFadeIn = 1f, float timeToFadeOut = 1f)
    {
        _instance.PrivateFadeToSong(audioClip, timeToFadeIn, timeToFadeOut);
    }
}
