using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightControl_DW : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 5f;

    [Header("Tilt Settings")]
    public float tiltAmount = 15f;   // Maximum tilt angle
    public float tiltSpeed = 5f;     // How quickly it eases

    [Header("Movement Limits")]
    public float maxDistanceFromOrigin = 20f;

    private Rigidbody rb;

    private float horizontal;
    private float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ===== READ INPUT =====
        horizontal = 0f;
        vertical = 0f;

        if (Input.GetKey(KeyCode.W))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.S))
            vertical = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontal = 1f;
        else if (Input.GetKey(KeyCode.A))
            horizontal = -1f;

        // ===== SMOOTH TILT =====
        float targetTiltX = -vertical * tiltAmount;   // Pitch
        float targetTiltZ = -horizontal * tiltAmount; // Bank

        Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * tiltSpeed
        );
    }

    void FixedUpdate()
    {
        // ===== SMOOTH MOVEMENT =====
        Vector3 targetVelocity = new Vector3(
            horizontal * moveSpeed,
            vertical * moveSpeed,
            rb.velocity.z
        );

        rb.velocity = Vector3.Lerp(
            rb.velocity,
            targetVelocity,
            Time.fixedDeltaTime * acceleration
        );

        Vector3 clampedPosition = rb.position;

        if (clampedPosition.magnitude > maxDistanceFromOrigin)
        {
            clampedPosition = clampedPosition.normalized * maxDistanceFromOrigin;
            rb.position = clampedPosition;

            // Optional: stop outward velocity so it doesn't fight the boundary
            rb.velocity = Vector3.zero;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, maxDistanceFromOrigin);
    }
}