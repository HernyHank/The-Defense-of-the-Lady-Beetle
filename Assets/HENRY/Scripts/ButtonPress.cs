using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ButtonPress : MonoBehaviour
{
    public enum ButtonType
    {
        Door,
        Turret
    }

    public Animator animator;
    public int buttonNumber = 0;
    // Start is called before the first frame update
    [Header("Settings")]
    public ButtonType typeOfButton; // This creates the dropdown in the Inspector
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        /*Debug.Log($"[VR] {hand.name} started hovering over {gameObject.name}");*/

        // Check the type: If it's NOT a Door, we call the parent controller
        if (typeOfButton != ButtonType.Door)
        {
            TurretMonitorController Foo = this.GetComponentInParent<TurretMonitorController>();
            if (Foo != null)
            {
                Foo.OnPress(buttonNumber);
            }
        }
        else
        {
            /*Debug.Log("Button is a Door type - Skipping Turret Controller.");*/
        }

        if (animator != null)
        {
            animator.SetBool("buttonIsPressed", true);
        }
    }

    // Rest of your SteamVR methods...
/*    private void OnHandHoverEnd(Hand hand) => Debug.Log($"[VR] {hand.name} stopped hovering.");
    private void OnAttachedToHand(Hand hand) => Debug.Log($"[VR] Attached to {hand.name}!");
    private void OnDetachedFromHand(Hand hand) => Debug.Log("[VR] Detached from hand.");*/

}
