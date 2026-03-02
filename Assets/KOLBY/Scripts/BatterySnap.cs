using UnityEngine;
using Valve.VR.InteractionSystem;

public class BatterySnap : MonoBehaviour
{
    public BatterySnapZone snapZone;  // drag your snap zone here in inspector
    private Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnDetachedFromHand(Hand hand)
    {
        if (snapZone != null)
        {
            snapZone.TrySnap();
        }
    }

    //testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            snapZone.TrySnap();
        }
    }
}
