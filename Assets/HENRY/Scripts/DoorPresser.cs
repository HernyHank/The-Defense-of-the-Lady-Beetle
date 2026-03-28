using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;


public class DoorPresser : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean bIsHeld;
    public TextMeshProUGUI doorOption;

    // Update is called once per frame
    void Update()
    { //debug
/*        if (bIsHeld.GetState(handType) == true)
        {
            animator.SetBool("buttonIsPressed", true);
        }*/

/*        Debug.Log("Button is pressed: " + animator.GetBool("buttonIsPressed"));
        Debug.Log("Player is in box: " + !animator.GetBool("playerHasExited"));
        Debug.Log("Door is open: " + animator.GetBool("doorIsOpen"));*/

    }

    /*    private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && animator.GetBool("buttonIsPressed") == false)
            {
                doorOption.SetText("Hold B to Open");
                doorOption.gameObject.SetActive(true);

                if (bIsHeld.GetStateDown(handType) == true)
                {
                    animator.SetBool("doorIsOpen", true);
                }
            } 
            else
            {
                doorOption.gameObject.SetActive(false);
            }

        }*/


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("playerHasExited", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("playerHasExited", true);
        }
    }

    public void buttonFinishedPressing()
    {
        animator.SetBool("buttonIsPressed", true);
    }

    public void doorFinishedOpening()
    {
        animator.SetBool("doorIsOpen", true);
    }
    public void doorFinishedClosing()
    {
        animator.SetBool("buttonIsPressed", false);
    }
}
