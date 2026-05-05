using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 20f;        // How fast asteroid moves toward player
    public float driftForce = 5f;        // Speed after collision
    public float spinForce = 2f;         // Angular velocity after collision

    private Rigidbody rb;
    private bool hasHitPlayer = false;
    public int bounciness = 5;

    AudioManager audioManager;
    AudioClip sploder;
    AudioClip kackleDeath;
    AudioClip kackleDeath1;
    AudioClip kackleDeath2;
    AudioClip kackleDeath3;

    public GameObject explosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();

        sploder = audioManager.FetchClip("SFX/ExplosionsFire/ExplosionSFX_01");
        kackleDeath = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_1");
        kackleDeath1 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_2");
        kackleDeath2 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_4");
        kackleDeath3 = audioManager.FetchClip("Dialogue/6. Retaliation/Retaliation_JoanFustrated/Retaliation_PirateDeath_5");

        // Initially move manually, not physics
        rb.isKinematic = false;
    }

    void Update()
    {
        if (!hasHitPlayer)
        {
            // Move negatively on Z axis (toward player)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip")) // && !hasHitPlayer
        {
            hasHitPlayer = true;

            // Enable physics
            rb.isKinematic = false;

            // Optional: make it float away from the ship
            Vector3 driftDir = (transform.position - collision.transform.position).normalized;
            rb.velocity = driftDir * driftForce * bounciness;

            // Optional: spin the asteroid
            rb.angularVelocity = Random.onUnitSphere * spinForce;
        }

    }

/*    private void OnTriggerStay(Collider other)
    {
        Debug.Log("I feel something enter");
        Debug.Log("Other's tag is: " + other.tag);
        if (other.CompareTag("Gun") && JoystickManager.Instance.button2)
        {
            Debug.Log("I undersatnd what is happening to me");
            // Spawn explosion at this object's current position (last known coordinates)
            SpawnExplosionAt(transform.position);
            // Hide/disable the ship
            this.gameObject.SetActive(false);
        }
    }*/
    private void SpawnExplosionAt(Vector3 position)
    {
        if (explosionPrefab == null)
        {
            Debug.LogWarning("PirateDestroy_HM: explosionPrefab is not assigned.");
            return;
        }

        AudioClip[] clipArray = new AudioClip[] { kackleDeath, kackleDeath1, kackleDeath2, kackleDeath3 };

        foreach (AudioClip clip in clipArray)
        {
            if (clip == null)
            {
                Debug.LogWarning("PirateDestroy_HM: One of the kackleDeath clips is not assigned:: " + clip.name);
            }
        }
        AudioClip chosenClip = clipArray[Random.Range(0, clipArray.Length)];

        audioManager.PlaySFXOneShotOverAudio(sploder, 0.8f);
        audioManager.PlaySFXOneShotOverAudio(chosenClip, 0.8f);
        GameObject inst = Instantiate(explosionPrefab, position, Quaternion.identity);
        StartCoroutine(AutoDestroyExplosion(inst));


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
}