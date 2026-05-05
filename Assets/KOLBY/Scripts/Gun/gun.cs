using System.Collections;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PaintGun : MonoBehaviour
{
    public GameObject paintPrefab;
    public Transform shootPoint;
    public float shootForce = 10f;
    public SteamVR_Action_Boolean triggerAction;
/*    public Rigidbody rb;*/

    private Hand hand; // the hand holding the gun
    private int lifetime = 10;

/*    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.detectCollisions = false; // disable collisions while attached to hand
    }*/

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
        StartCoroutine(DestroyPaint(paint));
    }

    IEnumerator DestroyPaint(GameObject paint)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(paint);
    }

    // Called automatically by SteamVR

/*    public bool hasAttchedFirstTime = false;*/
    private void OnAttachedToHand(Hand attachedHand)
    {
        hand = attachedHand;
/*        if (!hasAttchedFirstTime)
        {
            rb.detectCollisions = true; // disable collisions while attached to hand
            hasAttchedFirstTime = true;
        }*/
    }

    private void OnDetachedFromHand(Hand detachedHand)
    {
        hand = null;
    }
}
