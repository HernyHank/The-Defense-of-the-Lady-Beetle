using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAppearanceScript_HM : MonoBehaviour
{
    private Renderer gunRenderer;
    EventController controller;

    void Start()
    {
        controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        if(controller != null)
        {
            Debug.Log("found" + controller.name);
        } else
        {
            Debug.Log("did NOT find controller");
        }
        gunRenderer = GetComponent<Renderer>();
        gunRenderer.enabled = false; // hide at start
    }

    void Update()
    {
        Debug.Log("Turret can shoot value: " + controller.turretCanShoot);
        if (Input.GetKeyDown(KeyCode.K) && controller.turretCanShoot)
        {
            gunRenderer.enabled = true;
           // Debug.Log("gun is shot");
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            gunRenderer.enabled = false;
        }
    }
}