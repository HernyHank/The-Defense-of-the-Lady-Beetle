using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public EventController eventController;
    private AudioSource audioSource;
    public AudioSource musicSource;

    void Awake() => audioSource = GetComponent<AudioSource>();

    //private bool _wasPlayingLastFrame;
/*    private void Update()*//*
    {
*//*        if (!audioSource.isPlaying && _wasPlayingLastFrame)
        {
            OnDialogueFinished?.Invoke(); // Send the message!
        }
        _wasPlayingLastFrame = audioSource.isPlaying;*//*
    }*/



    public void Play(AudioClip clip)
    {
        audioSource.clip = clip; // 1. Assign the clip to the "player"
        audioSource.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayDialogueSequence(AudioClip clip, float volume)
    {
        eventController.dialogueActive = true;
        audioSource.Stop();
        audioSource.volume = volume;
        StartCoroutine(PlayAndWait(clip));
        //Debug.Log("Coroutine started");

        // Strategy A: Wait for the literal duration of the clip

    }

    IEnumerator PlayAndWait(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(clip.length);
        //Debug.Log("Dialogue finished, sending message to event controller");

        eventController.audioClipIndex++;
        eventController.dialogueActive = false;
    }

    public AudioClip FetchClip(string path)
    {
        AudioClip myClip = Resources.Load<AudioClip>(path);
        return myClip;
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayMusicClip(AudioClip clip)
    {
        StopMusic();
        musicSource.clip = clip;
        musicSource.Play();
        FadeAudio(musicSource, 1f, 0.3f); // Example: Fade in over 2 seconds to full volume
    }

    public IEnumerator FadeAudio(AudioSource source, float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            // Smoothly interpolate the volume
            source.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null; // Wait for the next frame
        }

        source.volume = targetVolume;

        // Optional: Stop the audio entirely if we faded to zero
        if (targetVolume <= 0) source.Stop();
    }

    public void StopMusic()
    {
        FadeAudio(musicSource, 1f, 0f); // Example: Fade out over 2 seconds
    }   

    public void StopEverything()
    {
        StopAudio();
        StopMusic();
    }

    public void PlaySFXOneShot(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

}
