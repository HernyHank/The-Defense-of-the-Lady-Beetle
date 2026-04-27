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
    public EventController controller;
    public int doorOpenCount = 0;

    // Update is called once per frame

    public enum DoorType
    {
        Regular,
        Airlock
    }

    public DoorType doorType = DoorType.Regular;

    private void Start()
    {
        if(doorType == DoorType.Airlock)
        {
            controller = FindObjectOfType<EventController>();
        }
    }
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
            //Debug.Log("Player has exited the door trigger area.");
            animator.SetBool("playerHasExited", true);
            /*if (doorType != DoorType.Airlock)
            {
                animator.SetBool("playerHasExited", true);
            }*/
        }
    }

    public void buttonFinishedPressing()
    {
        animator.SetBool("buttonIsPressed", true);
        if (doorType == DoorType.Airlock && controller.currentRoom == "Outside")
        {
            controller.outsideAirlockIsOpen = true;
        }
    }

    public void doorFinishedOpening()
    {
        animator.SetBool("doorIsOpen", true);    
    }
    public void doorFinishedClosing()
    {
        animator.SetBool("buttonIsPressed", false);
        if (doorType == DoorType.Airlock && controller.currentRoom == "Outside")
        {
            controller.outsideAirlockIsOpen = false;
        }
    }
}
