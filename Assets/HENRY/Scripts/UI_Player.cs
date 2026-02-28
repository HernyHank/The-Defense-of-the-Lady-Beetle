using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean bIsHeld;
    public TextMeshProUGUI bText;

    // Update is called once per frame
    void Update()
    {
        if (bIsHeld.GetState(handType) == true)
        {
            bText.gameObject.SetActive(true);
        }
        else
        {
            bText.gameObject.SetActive(false);
        }
    }
}
