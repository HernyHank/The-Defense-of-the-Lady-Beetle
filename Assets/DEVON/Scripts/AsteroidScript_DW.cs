using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 20f;        // How fast asteroid moves toward player
    public float driftForce = 5f;        // Speed after collision
    public float spinForce = 2f;         // Angular velocity after collision

    private Rigidbody rb;
    private bool hasHitPlayer = false;
    public int bounciness = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
}