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

    private Coroutine dialogueCoroutine;

    void Awake() => audioSource = GetComponent<AudioSource>();

/*    //private bool _wasPlayingLastFrame;
*//*    private void Update()*//*//*
    {
*//*        if (!audioSource.isPlaying && _wasPlayingLastFrame)
        {
            OnDialogueFinished?.Invoke(); // Send the message!
        }
        _wasPlayingLastFrame = audioSource.isPlaying;*//*
    }*/



    public void Play(AudioClip clip, bool overrideOrNah)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager.Play called with null clip.");
            return;
        }

        bool wasPlaying = audioSource.isPlaying;
        Debug.Log($"AudioManager.Play requested: clip='{clip.name}' length={clip.length} override={overrideOrNah} isPlayingBefore={wasPlaying}");

        // If we need to force (override) playback, stop current and start the new clip
        if (overrideOrNah)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = clip;
            audioSource.Play();

            Debug.Log($"AudioManager.Play: override -> started '{clip.name}', isPlayingAfter={audioSource.isPlaying}");
            return;
        }

        // Non-override behavior:
        // - If the audioSource is not playing we can assign the clip and Play()
        // - If it IS already playing, don't clobber audioSource.clip (that can interrupt current playback).
        //   PlayOneShot is used to play the requested sound without replacing the currently assigned clip.
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log($"AudioManager.Play: started '{clip.name}' on idle source, isPlayingAfter={audioSource.isPlaying}");
        }
        else
        {
            // Play as one-shot so we don't replace the currently playing clip
            /*audioSource.PlayOneShot(clip);
            Debug.Log($"AudioManager.Play: source already playing -> PlayOneShot('{clip.name}')");*/
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayDialogueSequence(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlayDialogueSequence called with null clip.");
            return;
        }
        //eventController.audioClipIndex++;
        Debug.Log("Audioclip index after increment: " + eventController.audioClipIndex);
        Debug.Log("AudioCLip list count: " + eventController.audioClipList.Count);
        eventController.dialogueActive = true;
        //audioSource.Stop();
        audioSource.volume = volume;

        // stop any existing dialogue coroutine instance
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        dialogueCoroutine = StartCoroutine(PlayAndWait(clip));
        //Debug.Log("Coroutine started");

        // Strategy A: Wait for the literal duration of the clip

    }

    IEnumerator PlayAndWait(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(clip.length);
        //Debug.Log("Dialogue finished, sending message to event controller");

        eventController.dialogueActive = false;

        // clear handle
        dialogueCoroutine = null;
    }

    public AudioClip FetchClip(String path)
    {
        AudioClip myClip = Resources.Load<AudioClip>(path);
        return myClip;
    }

    public void StopDialogue()
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        //audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayMusicClip(AudioClip clip)
    {
        //StopMusic();
        if (clip == null)
        {
            Debug.LogWarning("PlayMusicClip called with null clip.");
            return;
        }
        
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.volume = 0.5f; // Start silent for fade-in
        FadeAudio(musicSource, 1f, 0.5f); // Fade in to target volume
        Debug.Log("Music source play attempted");
        //StartCoroutine(FadeAudio(musicSource, 1f, 0.3f)); // Fade in
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
        //if (targetVolume <= 0) source.Stop();
    }

    public void StopMusic()
    {
        StartCoroutine(FadeAudio(musicSource, 1f, 0f)); // Fade out
    }   

    public void StopEverything()
    {
        StopDialogue();
        StopMusic();
    }

    public void PlaySFXOneShot(AudioClip clip, float volume)
    {
        if(!audioSource.isPlaying)
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlaySoundFromSource(AudioSource source, AudioClip clip, float volume)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySoundFromSource called with null clip.");
            return;
        }
        source.PlayOneShot(clip, volume);
    }

}
