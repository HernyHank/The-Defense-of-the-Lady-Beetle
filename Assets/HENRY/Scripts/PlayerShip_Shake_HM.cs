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

    void Start()
    {
        originalRotation = transform.localRotation;
        pitchAxis = transform.right; // local X axis = nose-down pitch
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
            TriggerShake(5f,5f);
        }

    }

    public void TriggerShake(float duration, float intensity)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        shakeTimer = duration;
        controller.DamageShip(10);
    }
}