using UnityEngine;
using Valve.VR.InteractionSystem;

public class BatterySnap : MonoBehaviour
{
    private BatterySnapZone currentZone;

    public enum BatteryState
    {
        Full,
        Empty,
        Torpedo
    }

    public BatteryState currentState = BatteryState.Full;

    private Interactable interactable;
    private Rigidbody rb;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnDetachedFromHand(Hand hand)
    {
        if (currentZone != null)
        {
            currentZone.TrySnap(this);
        }
        else
        {
            // Normal drop
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    // Called by SnapZone
    public void SetCurrentZone(BatterySnapZone zone)
    {
        currentZone = zone;
    }

    public void ClearZone(BatterySnapZone zone)
    {
        if (currentZone == zone)
        {
            currentZone = null;
        }
    }

    public void SetInteractable(bool enabled)
    {
        if (interactable != null)
        {
            interactable.enabled = enabled;
        }
    }
}