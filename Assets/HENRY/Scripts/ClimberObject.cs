using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class ClimberObject : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public int TouchedCount;
    public bool grabbing;

    private Hand.AttachmentFlags attachmentFlags;

    private void Awake()
    {
        // 2. Assign the value here once the script starts running
        attachmentFlags = Hand.defaultAttachmentFlags
            & (~Hand.AttachmentFlags.ParentToHand)
            & (~Hand.AttachmentFlags.SnapOnAttach)
            & (~Hand.AttachmentFlags.DetachOthers)
            & (~Hand.AttachmentFlags.VelocityMovement);
    }

    private void OnHandHoverBegin(Hand hand)
    {
       // Debug.Log($"[VR] {hand.name} started hovering over {gameObject.name}");
        TouchedCount++;
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
            // This is the "Magic" call that makes it attached
            hand.AttachObject(this.gameObject, startingGrabType, attachmentFlags);
            VRPlayerMovement script = hand.GetComponentInParent<VRPlayerMovement>();
            script.climbingToggle(hand);
            //turn Rigid body on
        }
    }

    private void OnHandHoverEnd(Hand hand)
    {
        //Debug.Log($"[VR] {hand.name} stopped hovering over {gameObject.name}");
        TouchedCount--;
    }

    private void HandAttachedUpdate(Hand hand)
    {
        // This runs every frame WHILE the object is held
        if (hand.IsGrabEnding(gameObject))
        {
            hand.DetachObject(gameObject);
            VRPlayerMovement script = hand.GetComponentInParent<VRPlayerMovement>();
            script.climbingToggle(hand);
            //turn rigid body off
        }
    }


}