using UnityEngine;
using Valve.VR.InteractionSystem;

public class BatterySnap_DW : MonoBehaviour
{
    public BatterySnapZone snapZone;
    //public float ejectDelay = 3f;
    //public float ejectForce = 5f;

    //private GameObject snappedBattery;

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
