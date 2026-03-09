using UnityEngine;
using Valve.VR.InteractionSystem;

public class BatterySnap : MonoBehaviour
{
    public BatterySnapZone snapZone;

    private void OnDetachedFromHand(Hand hand)
    {
      

        if(snapZone.IsBatteryInZone(this.gameObject))
{
            snapZone.TrySnap();
        }
else
        {
            // Normal drop
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
