using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAppearanceScript_HM : MonoBehaviour
{
    private Renderer gunRenderer;

    void Start()
    {
        gunRenderer = GetComponent<Renderer>();
        gunRenderer.enabled = false; // hide at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
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