using System.Collections;
using UnityEngine;

public class PlayerShip_Shake_HM : MonoBehaviour
{
    public VRPlayerMovement playerScript;
    public EventController controller;

    [Header("Shake Settings")]
    public float shakeDuration = 2f;
    public float shakeIntensity = 14f;
    public float shakeFrequency = 14f;

    [Header("Horizontal Wobble Settings")]
    public float wobbleIntensity = 4f;
    public float wobbleFrequency = 3f;

    private float shakeTimer;
    private Quaternion originalRotation;
    private Vector3 pitchAxis;

    public AudioManager audioManager;

    AudioClip[] steroidClipArray;
    AudioClip[] pirateClipArray;
    AudioClip[] bothClipArray;

    void Start()
    {
        originalRotation = transform.localRotation;
        pitchAxis = transform.right; // local X axis = nose-down pitch
            audioManager = FindObjectOfType<AudioManager>();

        AudioClip asteroidSound = audioManager.FetchClip("Dialogue/5. Evasive/Evasive_JoanWhenTheAsteroidsHit/Evasive_JoanWhenAsteroidHit01");
        AudioClip asteroidSound1 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive_JoanWhenTheAsteroidsHit/Evasive_JoanWhenAsteroidHit02");
        AudioClip asteroidSound2 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive_JoanWhenTheAsteroidsHit/Evasive_JoanWhenAsteroidHit03");
        AudioClip asteroidSound3 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive__Big Tin Cannister of Polywoggle orbs_");
        AudioClip asteroidSound4 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive__Every Day_s the same and it sucks_");
        AudioClip asteroidSound5 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive__Steer, joan, Steer!_");

        //Just pirates (this was the death sound)
/*        AudioClip kackleDeath1 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_2");
        AudioClip kackleDeath2 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_4");
        AudioClip kackleDeath3 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_5");*/

        //Both
        AudioClip bothSound = audioManager.FetchClip("Dialogue/5. Evasive/Evasive_KapeyFrightened01");
        AudioClip bothSound1 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive_KapeyFrightened02");
        AudioClip asteroidSound6 = audioManager.FetchClip("Dialogue/5. Evasive/Evasive__WAAAEEEAAAGGH- watch out!_");

        steroidClipArray = new AudioClip[] { asteroidSound, asteroidSound1, asteroidSound2, asteroidSound3, asteroidSound4, asteroidSound5, bothSound1, bothSound, asteroidSound6 };
        pirateClipArray = new AudioClip[] { bothSound1, bothSound, asteroidSound6 };

        foreach (AudioClip clip in pirateClipArray)
        {
            if (clip == null)
            {
                Debug.LogWarning("PirateDestroy_HM: One of the kackleDeath clips is not assigned:: " + clip.name);
            }
        }
    }

    bool hasShaken = false;

    void Update()
    {
        if (shakeTimer > 0f)
        {
            playerScript.shipIsShaking = true;
            //Debug.Log(playerScript.shipIsShaking);
            hasShaken = true;
            shakeTimer -= Time.deltaTime;
            float progress = 1f - (shakeTimer / shakeDuration);

            // Exponential decay
            float damping = Mathf.Exp(-3f * progress);

            // Primary nose-down pitch
            float pitchAngle = -Mathf.Sin(progress * shakeFrequency) * shakeIntensity * damping;
            Quaternion pitchOffset = Quaternion.AngleAxis(pitchAngle, pitchAxis);

            // Secondary wobble: fade in horizontal roll over time
            float wobbleFactor = Mathf.Sin(progress * Mathf.PI); // 0 at start, 1 at middle, 0 at end
            float rollAngle = Mathf.Sin(progress * wobbleFrequency * Mathf.PI * 2f) * wobbleIntensity * damping * wobbleFactor;
            Quaternion rollOffset = Quaternion.AngleAxis(rollAngle, transform.forward); // roll around local Z

            // Combine pitch + roll
            transform.localRotation = originalRotation * pitchOffset * rollOffset;
        }
        else if (shakeTimer <= 0 && hasShaken == true)
        {
            
            playerScript.shipIsShaking = false;
            shipIsShaking = false;
            //Debug.Log(playerScript.shipIsShaking);
            hasShaken = false;
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                originalRotation,
                Time.deltaTime * 4f
            );
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            TriggerShake(5f,5f, 0);
        }

    }

    bool shipIsShaking = false;
    public void TriggerShake(float duration, float intensity, int mode)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        shakeTimer = duration;
        controller.DamageShip(10f);

        StartCoroutine(TriggerJoanFrustrationNoise(mode));
    }

    void setShipToIsShaking()
    {
        shipIsShaking = true;
    }

    IEnumerator TriggerJoanFrustrationNoise(int mode)
    {
               yield return new WaitForSeconds(0.5f);
        //Just asteroids

        if(shipIsShaking == false)
        {
            if (mode == 0)
            {
                AudioClip chosenClip = steroidClipArray[Random.Range(0, steroidClipArray.Length)];
                audioManager.Play(chosenClip);
            }
            if (mode == 1)
            {
                AudioClip chosenClip = pirateClipArray[Random.Range(0, pirateClipArray.Length)];
                audioManager.Play(chosenClip);
            }// Exit if the ship is no longer shaking
        }
        setShipToIsShaking();
    }
}