using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PirateDestroy_HM : MonoBehaviour
{
    private int pirateShotLayer;
    public EventController controller;
    Animator orbitAnimator;
    Animator bodyAnimator;

    AudioManager audioManager;
    AudioSource audioSource;

    [Header("VFX")]
    public GameObject explosionPrefab;

    public PlayerShip_Shake_HM playerShakeScript;   

    // Prevent multiple explosions from the same pirate instance
    private bool hasBeenDestroyed = false;

    public bool playerShipShouldShake = false;

    List<AudioClip> kackleIntimidationSounds = new List<AudioClip>();

    private void Awake()
    {
        playerShakeScript = FindObjectOfType<PlayerShip_Shake_HM>();
        controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        orbitAnimator = this.gameObject.transform.parent.GetComponent<Animator>();
        bodyAnimator = this.gameObject.GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        audioSource = GetComponent<AudioSource>();

        for (int i = 1; i < 9; i++)
        {
            //Debug.Log("For loop entered");
            AudioClip clip = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_KackleIntimidation/Ambush_KackleIntimidation0" + i);

            if (clip != null)
            {
                Debug.Log(" Successfully loaded clip: " + clip.name);
            }
            else
            {
                Debug.LogWarning(" Failed to load clip: Dialogue/4. Ambush/Ambush_KackleIntimidation/Ambush_KackleIntimidation0" + i);
            }
            kackleIntimidationSounds.Add(clip);
        }
    }

    private void Update()
    {
        if (playerShipShouldShake)
        {
            playerShakeScript.TriggerShake(1f, 10f, 1);
        }
    }

    int explosionCount = 0; // For debugging

    void OnTriggerStay(Collider other)
    {
        // Guard: one-shot only

        // Only trigger once when conditions met
        if (other.CompareTag("Gun") && controller != null && controller.turretCanShoot && JoystickManager.Instance.button2)
        {
            explosionCount++;
            

            // Spawn explosion at this object's current position (last known coordinates)
            SpawnExplosionAt(transform.position);
            Debug.Log("collPirate ship is destroyed! Explosion count" + explosionCount);

            orbitAnimator.Rebind();
            orbitAnimator.Update(0f);
            bodyAnimator.Rebind();
            bodyAnimator.Update(0f);

            getParentAndSend();

            // Hide/disable the ship
            this.gameObject.SetActive(false);
        }

        return;
    }

    public void PlayKackleIntimidation()
    {
        AudioClip clip = kackleIntimidationSounds[Random.Range(0, kackleIntimidationSounds.Count)];
        audioManager.PlaySFXOneShot(clip, 0.8f);
    }

    private void SpawnExplosionAt(Vector3 position)
    {
        if (explosionPrefab == null)
        {
            Debug.LogWarning("PirateDestroy_HM: explosionPrefab is not assigned.");
            return;
        }
        AudioClip sploder = audioManager.FetchClip("SFX/ExplosionsFire/ExplosionSFX_01");
        AudioClip kackleDeath = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_1");
        AudioClip kackleDeath1 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_2");
        AudioClip kackleDeath2 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_4");
        AudioClip kackleDeath3 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_5");

        AudioClip[] clipArray = new AudioClip[] { kackleDeath, kackleDeath1, kackleDeath2, kackleDeath3 };

        foreach (AudioClip clip in clipArray)
        {
            if (clip == null)
            {
                Debug.LogWarning("PirateDestroy_HM: One of the kackleDeath clips is not assigned:: " + clip.name);
            }
        }
        AudioClip chosenClip = clipArray[Random.Range(0, clipArray.Length)];
        audioManager.PlaySoundFromSource(audioSource, chosenClip, 0.8f);
        audioManager.PlaySoundFromSource(audioSource, sploder, 0.5f);

        audioManager.PlaySFXOneShot(sploder, 0.8f);
        audioManager.PlaySFXOneShot(chosenClip, 0.8f);
        GameObject inst = Instantiate(explosionPrefab, position, Quaternion.identity);
        StartCoroutine(AutoDestroyExplosion(inst));
        StartCoroutine(DelayedJoanReaction());
    }

    private IEnumerator AutoDestroyExplosion(GameObject explosion)
    {
        if (explosion == null) yield break;

        // Try to compute a safe lifetime from any child ParticleSystem(s)
        float waitTime = 5f; // fallback

        ParticleSystem ps = explosion.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            float duration = main.duration;
            float startLifetime = 0f;

            // Handle MinMaxCurve safely
            try
            {
                if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants)
                    startLifetime = main.startLifetime.constantMax;
                else
                    startLifetime = main.startLifetime.constant;
            }
            catch
            {
                // fall back to a small value if anything unexpected happens
                startLifetime = 1f;
            }

            waitTime = duration + startLifetime + 0.5f;
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(explosion);
    }

    private IEnumerator DelayedJoanReaction()
    {
        AudioClip joanHitsTarget = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget02");
        AudioClip joanHitsTarget1 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget03");
        AudioClip joanHitsTarget2 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget04");
        AudioClip joanHitsTarget3 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget05");
        AudioClip joanHitsTarget4 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget08");
        AudioClip joanHitsTarget5 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget09");
        AudioClip joanHitsTarget6 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget10");
        AudioClip joanHitsTarget7 = audioManager.FetchClip("Dialogue/4. Ambush/Ambush_JoanHitsTarget/Ambush_JoanHitsTarget12");
        AudioClip[] clipArray = new AudioClip[] { joanHitsTarget, joanHitsTarget1, joanHitsTarget2, joanHitsTarget3, joanHitsTarget4, joanHitsTarget5, joanHitsTarget6, joanHitsTarget7 };
        yield return new WaitForSeconds(0.5f); // Delay before Joan reacts

        AudioClip chosenClip = clipArray[Random.Range(0, clipArray.Length)];
        audioManager.PlaySFXOneShot(chosenClip, 0.8f);

    }

    public void getParentAndSend()
    {

        orbitAnimator.Rebind();
        orbitAnimator.Update(0f);
        bodyAnimator.Rebind();
        bodyAnimator.Update(0f);
        // Safely walk up the hierarchy with a hard limit to avoid infinite loops
        Transform currentParent = transform;
        const int maxLevelsUp = 10;
        int levels = 0;

        // Look for an ancestor whose name matches "CameraJoint (N)"
        Regex cameraJointRegex = new Regex(@"^CameraJoint\s*\((\d+)\)$");

        while (currentParent != null && levels < maxLevelsUp)
        {
            Match m = cameraJointRegex.Match(currentParent.name);
            if (m.Success)
            {
                if (int.TryParse(m.Groups[1].Value, out int index))
                {
                    //Debug.Log("sending destroy ship of index " + index);
                    controller?.DestroyShip(index);
                    return;
                }

                Debug.LogWarning($"getParentAndSend: failed to parse index from CameraJoint match in '{currentParent.name}'");
                break;
            }

            currentParent = currentParent.parent;
            levels++;
        }

        // Fallback: if no exact match found, try to extract the first integer anywhere in the final ancestor name
        if (currentParent != null)
        {
            string targetName = currentParent.name;
            Debug.Log($"getParentAndSend fallback using name='{targetName}'");

            Match fallbackMatch = Regex.Match(targetName, @"\d+");
            if (fallbackMatch.Success && int.TryParse(fallbackMatch.Value, out int fallbackIndex))
            {
                Debug.Log("sending destroy ship of index (fallback) " + fallbackIndex);
                controller?.DestroyShip(fallbackIndex);
                return;
            }

            Debug.LogWarning($"getParentAndSend: could not parse index from '{targetName}'. No numeric token found.");
            return;
        }

        // If we exhausted the ancestry without finding a usable name
        Debug.LogWarning("getParentAndSend: reached top of hierarchy without finding a CameraJoint-like ancestor.");
    }

}