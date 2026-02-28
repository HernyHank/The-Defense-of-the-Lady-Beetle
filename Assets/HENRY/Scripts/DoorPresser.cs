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
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("doorIsOpen", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && animator.GetBool("doorIsOpen") == false)
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

    }
}
