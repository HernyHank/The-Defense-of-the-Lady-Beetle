using UnityEngine;

public class Gun_DW : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject laserPrefab;    // Your laser prefab
    public Transform shootPoint;      // Where the laser comes out
    public float laserSpeed = 50f;    // Speed of laser

    [Header("VFX")]
    public GameObject particleObject; // Particle system GameObject to activate on Y

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Shoot();

            if (particleObject != null)
            {
                // Ensure the GameObject is active
                particleObject.SetActive(true);

                // If it has a ParticleSystem, play it (handles prefab already active or activated here)
                ParticleSystem ps = particleObject.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
                else
                {
                    ParticleSystem psChild = particleObject.GetComponentInChildren<ParticleSystem>();
                    if (psChild != null) psChild.Play();
                }
            }
        }

       // Debug.DrawRay(shootPoint.position, shootPoint.forward * 10f, Color.red, 2f);
    }

    void Shoot()
    {
        if (laserPrefab == null || shootPoint == null) return;

        // Instantiate the laser at shootPoint position
        GameObject laser = Instantiate(laserPrefab, shootPoint.position, Quaternion.identity);

        // Align the laser with shootPoint forward
        laser.transform.forward = shootPoint.forward;

        // Set velocity along the forward axis
        Rigidbody rb = laser.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = laser.transform.forward * laserSpeed;
        }

        // Optional: destroy after some time to clean up
        Destroy(laser, 10f);
    }
}