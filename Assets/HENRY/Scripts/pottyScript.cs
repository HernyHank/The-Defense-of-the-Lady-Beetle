using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pottyScript : MonoBehaviour
{

    private Transform peeLocation;
    private Transform finishLocation;
    private bool isPeeing;
    private bool isJustAfterFinishing = false;
    public int peeCounter = 0;

    public AudioClip clip;
    public AudioSource peeAudioSource;

    VRPlayerMovement script;

    public bool eventComplete = false;

    private void Start()
    {
        peeLocation = transform.Find("peeLocation");
        finishLocation = transform.Find("finishLocation");

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isPeeing && !isJustAfterFinishing)
        {
            if (other.CompareTag("Player"))
            {
                VRPlayerMovement script = other.gameObject.GetComponent<VRPlayerMovement>();
                script.SetUIText("Hold B to Pee", true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isPeeing)
        {
            script = other.gameObject.GetComponent<VRPlayerMovement>();
            if (script.GetBState())
            {
                // LOCK THIS IMMEDIATELY so it doesn't fire next frame
                isPeeing = true;

                Vector3 pottyPosition = peeLocation.transform.position;
                Vector3 pottyRotation = peeLocation.transform.rotation.eulerAngles;

                script.SetUIText("", false); // Cleared text
                script.SetJoanTransform(pottyPosition, pottyRotation, false);

                // CORRECT PLAY SEQUENCE
                peeAudioSource.clip = clip;
                peeAudioSource.volume = 0;
                peeAudioSource.Play();

                StartCoroutine(PottyTimer(5));
                StartCoroutine(FadeAudio(peeAudioSource, 0.5f, 1f));
            }
        }
    }

    public IEnumerator PottyTimer(float waitTime)
    {
        Vector3 finishPosition = finishLocation.position;
        Vector3 finishRotation = finishLocation.eulerAngles;

        // Wait for most of the time
        yield return new WaitForSeconds(waitTime - 0.5f);

        // Start the fade out
        StartCoroutine(FadeAudio(peeAudioSource, 0.5f, 0f));

        // Wait for only the REMAINING 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        isPeeing = false;
        peeCounter++;
        script.SetJoanTransform(finishPosition, finishRotation, true);
        isJustAfterFinishing = true;
        eventComplete = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VRPlayerMovement script = other.gameObject.GetComponent<VRPlayerMovement>();
            script.SetUIText("You shouldn't see this", false);

            isJustAfterFinishing = false;

        }
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

}
