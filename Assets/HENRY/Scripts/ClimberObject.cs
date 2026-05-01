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
    public AudioManager audioManager;

    private Hand.AttachmentFlags attachmentFlags;

    List<AudioClip> ladderSounds = new List<AudioClip>();

    private void Awake()
    {
        //Debug.Log("CLimber object woken up");   
        // 2. Assign the value here once the script starts running
        attachmentFlags = Hand.defaultAttachmentFlags
            & (~Hand.AttachmentFlags.ParentToHand)
            & (~Hand.AttachmentFlags.SnapOnAttach)
            & (~Hand.AttachmentFlags.DetachOthers)
            & (~Hand.AttachmentFlags.VelocityMovement);

        for (int i = 1; i < 9; i++)
        {
            //Debug.Log("For loop entered");
            AudioClip clip = audioManager.FetchClip("Dialogue/3. The Calm/LadderSounds0" + i);
            
            if (clip != null)
            {
                Debug.Log("[ClimberObject] Successfully loaded clip: " + clip.name);
            } else
            {
                Debug.LogWarning("[ClimberObject] Failed to load clip: Dialogue/3. The Calm/LadderSounds0" + i);
            }
                ladderSounds.Add(clip);
        }
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
            script.climbingToggle(hand, true);
            AudioClip clip = ladderSounds[Random.Range(0, ladderSounds.Count)];
            audioManager.PlaySFXOneShot(clip, 0.8f);
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
            script.climbingToggle(hand, false);
            //turn rigid body off
        }
    }


}