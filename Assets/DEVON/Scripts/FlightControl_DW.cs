using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightControl_DW : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;

    //[Header("Animator References")]
    //public Animator flightAnimator; // Animator on the spaceship

    [Header("Tilt Settings")]
    public float tiltAmount = 15f;     // How much the ship tilts
    public float tiltSpeed = 5f;       // How fast it eases

    void Start()
    {
        // Cache the 3D Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        // ===== VERTICAL MOVEMENT (Y AXIS) =====
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1f;
            Debug.Log("W is pressed");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1f;
            Debug.Log("S is pressed");
        }

        // ===== HORIZONTAL MOVEMENT (X AXIS) =====
        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
            Debug.Log("D is pressed");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
            Debug.Log("A is pressed");
        }

        // Apply movement ONCE (no overwriting)
        rb.velocity = new Vector3(
            horizontal * moveSpeed,
            vertical * moveSpeed,
            rb.velocity.z // keeps current Z velocity unchanged
        );


        // Calculate tilt based on input
        float targetTiltX = -vertical * tiltAmount;   // Pitch (W/S)
        float targetTiltZ = -horizontal * tiltAmount; // Bank (A/D)

        // Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0f, targetTiltZ);

        // Smoothly ease toward the target rotation
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * tiltSpeed
        );

    }
}