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

    public Material normalMaterial;
    public Material warningMaterial;
    public Material inactiveMaterial;

    private Renderer myRenderer;

    public Animator animator;
    public int buttonNumber = 0;
    // Start is called before the first frame update
    [Header("Settings")]
    public ButtonType typeOfButton; // This creates the dropdown in the Inspector
    void Awake()
    {
        // 2. Grab the Renderer component once at the start
        animator = GetComponent<Animator>();
        myRenderer = GetComponent<Renderer>();
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

    public void SetMaterial(string materialType)
    {
        // 3. Switch based on the string sent by the button press
        switch (materialType)
        {
            case "Normal":
                myRenderer.material = normalMaterial;
                break;
            case "Warning":
                myRenderer.material = warningMaterial;
                break;
            case "Inactive":
                myRenderer.material = inactiveMaterial;
                break;
            default:
                Debug.LogWarning("Material type not recognized!");
                break;
        }
    }

    // Rest of your SteamVR methods...
    /*    private void OnHandHoverEnd(Hand hand) => Debug.Log($"[VR] {hand.name} stopped hovering.");
        private void OnAttachedToHand(Hand hand) => Debug.Log($"[VR] Attached to {hand.name}!");
        private void OnDetachedFromHand(Hand hand) => Debug.Log("[VR] Detached from hand.");*/

}
