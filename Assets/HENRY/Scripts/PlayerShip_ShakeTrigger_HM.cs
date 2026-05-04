using UnityEngine;

public class PlayerShip_ShakeTrigger_HM : MonoBehaviour
{
    
    private int asteroidLayer;
    private int laserLayer;

    public EventController controller;
    public float cooldownTime = 3f;
    private float cooldownTimer = 0f;

    void Start()
    {
        asteroidLayer = LayerMask.NameToLayer("Asteroid");
        laserLayer = LayerMask.NameToLayer("Laser");
    }

    void Update()
    {
        // Count down cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If still on cooldown ? do nothing
        if (cooldownTimer > 0f) return;

        int otherLayer = collision.gameObject.layer;

        // Check if it's Asteroid OR Laser
        if (otherLayer == asteroidLayer)
        {
            var shake = GetComponentInParent<PlayerShip_Shake_HM>();

            if (shake != null)
            {
                shake.TriggerShake(2f, 14f, 0);
            }

            // Start cooldown
            cooldownTimer = cooldownTime;
        }
    }
}