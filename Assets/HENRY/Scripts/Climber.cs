using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*[RequireComponent(typeof(Rigidbody))]
public class Climber : MonoBehaviour
{
    public ClimberObject RightHand;
    public ClimberObject LeftHand;
    public SteamVR_Action_Boolean ToggleGripButton;
    public SteamVR_Action_Pose position;
    public ConfigurableJoint ClimberHandle;

    private bool Climbing;
    private ClimberObject ActiveHand;

    void Update()
    {
        updateHand(RightHand);
        updateHand(LeftHand);
        if (Climbing)
        {
            ClimberHandle.targetPosition = -ActiveHand.transform.localPosition;//update collider for hand movment
        }
    }

    void updateHand(ClimberObject Hand)
    {
        if (Climbing && Hand == ActiveHand)//if is the hand used for climbing check if we are letting go.
        {
            if (ToggleGripButton.GetStateUp(Hand.handType))
            {
                ClimberHandle.connectedBody = null;
                Climbing = false;

                GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            if (ToggleGripButton.GetStateDown(Hand.handType) || Hand.grabbing)
            {
                Hand.grabbing = true;
                if (Hand.TouchedCount > 0)
                {
                    ActiveHand = Hand;
                    Climbing = true;
                    ClimberHandle.transform.position = Hand.transform.position;
                    GetComponent<Rigidbody>().useGravity = false;
                    ClimberHandle.connectedBody = GetComponent<Rigidbody>();
                    Hand.grabbing = false;
                }
            }
        }
    }
}*/



