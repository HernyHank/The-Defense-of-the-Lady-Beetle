using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Rigidbody))]
public class BatteryPhysicsGrab : MonoBehaviour
{
    public BatterySnapZone snapZone; // assign in inspector
    public float followStrength = 50f; // tweak for hand responsiveness

    private Rigidbody rb;
    private Hand attachedHand;
    private Interactable interactable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
    }

    private void FixedUpdate()
    {
        // Physics-based following
        if (attachedHand != null)
        {
            Vector3 targetPos = attachedHand.transform.position;
            Vector3 move = targetPos - rb.position;
            rb.velocity = move * followStrength * Time.fixedDeltaTime;

            // Optional: match rotation
            Quaternion targetRot = attachedHand.transform.rotation;
            Quaternion deltaRot = targetRot * Quaternion.Inverse(rb.rotation);
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180) angle -= 360;
            rb.angularVelocity = axis * angle * Mathf.Deg2Rad * followStrength * Time.fixedDeltaTime;
        }
    }

    // Called by SteamVR when hand grabs object
    private void OnAttachedToHand(Hand hand)
    {
        attachedHand = hand;

        rb.isKinematic = false;
        rb.useGravity = false; // optional, keep it controlled by hand
    }

    // Called by SteamVR when hand releases object
    private void OnDetachedFromHand(Hand hand)
    {
        attachedHand = null;

        if (snapZone != null && snapZone.IsBatteryInZone(gameObject))
        {
            BatterySnap battery = GetComponent<BatterySnap>();
            snapZone.TrySnap(battery);
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}