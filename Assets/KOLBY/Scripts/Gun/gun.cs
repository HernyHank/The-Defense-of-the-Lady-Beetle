using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PaintGun : MonoBehaviour
{
    public GameObject paintPrefab;
    public Transform shootPoint;
    public float shootForce = 10f;
    public SteamVR_Action_Boolean triggerAction;

    private Hand hand; // the hand holding the gun

    private void Update()
    {
        if (hand != null && triggerAction.GetStateDown(hand.handType))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject paint = Instantiate(paintPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = paint.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
    }

    // Called automatically by SteamVR
    private void OnAttachedToHand(Hand attachedHand)
    {
        hand = attachedHand;
    }

    private void OnDetachedFromHand(Hand detachedHand)
    {
        hand = null;
    }
}
