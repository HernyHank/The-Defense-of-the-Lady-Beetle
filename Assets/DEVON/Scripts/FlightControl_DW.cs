using UnityEngine;

public class FlightControl_DW : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;   // Speed of the ship
    public float tiltAmount = 15f;  // Maximum tilt angle
    public float tiltSpeed = 5f;    // Tilt easing speed
    public float maxDistanceFromOrigin = 20f;

    private float horizontal;
    private float vertical;

    void Update()
    {
        // ===== READ INPUT =====
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // ===== MOVE SHIP =====
        Vector3 move = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
        transform.position += move;

        // Clamp to spherical boundary
       // if (transform.position.magnitude > maxDistanceFromOrigin)
        //{
        //    transform.position = transform.position.normalized * maxDistanceFromOrigin;
        //}

        // ===== SMOOTH TILT =====
        float targetTiltX = -vertical * tiltAmount;
        float targetTiltZ = -horizontal * tiltAmount;

        Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, maxDistanceFromOrigin);
    }
}