using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PirateDestroy_HM : MonoBehaviour
{
    private int pirateShotLayer;
    public EventController controller;
    Animator orbitAnimator;
    Animator bodyAnimator;

    private void Awake()
    {
        //controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        orbitAnimator = this.gameObject.transform.parent.GetComponent<Animator>();
        bodyAnimator = this.gameObject.GetComponent<Animator>();
    }

    void OnTriggerStay(Collider other)
    {
        if (JoystickManager.Instance.button2 && other.CompareTag("Gun") && controller.turretCanShoot)
        {
            Debug.Log("collPirate ship is destroyed!");

            orbitAnimator.Rebind();
            orbitAnimator.Update(0f);
            bodyAnimator.Rebind();
            bodyAnimator.Update(0f);
            getParentAndSend();
        }

        return;
    }

    public void getParentAndSend()
    {
        // Safely walk up the hierarchy with a hard limit to avoid infinite loops
        Transform currentParent = transform;
        const int maxLevelsUp = 10;
        int levels = 0;

        // Look for an ancestor whose name matches "CameraJoint (N)"
        Regex cameraJointRegex = new Regex(@"^CameraJoint\s*\((\d+)\)$");

        while (currentParent != null && levels < maxLevelsUp)
        {
            Match m = cameraJointRegex.Match(currentParent.name);
            if (m.Success)
            {
                if (int.TryParse(m.Groups[1].Value, out int index))
                {
                    Debug.Log("sending destroy ship of index " + index);
                    controller?.DestroyShip(index);
                    return;
                }

                Debug.LogWarning($"getParentAndSend: failed to parse index from CameraJoint match in '{currentParent.name}'");
                break;
            }

            currentParent = currentParent.parent;
            levels++;
        }

        // Fallback: if no exact match found, try to extract the first integer anywhere in the final ancestor name
        if (currentParent != null)
        {
            string targetName = currentParent.name;
            Debug.Log($"getParentAndSend fallback using name='{targetName}'");

            Match fallbackMatch = Regex.Match(targetName, @"\d+");
            if (fallbackMatch.Success && int.TryParse(fallbackMatch.Value, out int fallbackIndex))
            {
                Debug.Log("sending destroy ship of index (fallback) " + fallbackIndex);
                controller?.DestroyShip(fallbackIndex);
                return;
            }

            Debug.LogWarning($"getParentAndSend: could not parse index from '{targetName}'. No numeric token found.");
            return;
        }

        // If we exhausted the ancestry without finding a usable name
        Debug.LogWarning("getParentAndSend: reached top of hierarchy without finding a CameraJoint-like ancestor.");
    }

}