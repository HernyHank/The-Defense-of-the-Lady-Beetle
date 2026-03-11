using UnityEngine;

public class Laser_DW : MonoBehaviour
{
    [Header("Laser Settings")]
    public float speed = 50f; // How fast the laser moves

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Shoot along the spawn rotation's forward (matches shootPoint)
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Optional: ignore hitting the player
        // if (collision.gameObject.CompareTag("Player")) return;

        // Destroy laser on impact
        Destroy(gameObject);
    }
}
