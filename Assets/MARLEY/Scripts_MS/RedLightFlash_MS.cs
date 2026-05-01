using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightFlash_MS : MonoBehaviour
{
    public Light pointLight;
    public EventController controller;
    public KeyCode toggleKey = KeyCode.F;

    public float flashSpeed = 0.5f; // time between on/off

    private bool isFlashing = false;
    private Coroutine flashCoroutine;

    void Start()
    {
        controller = FindObjectOfType<EventController>();
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }

        SetWhiteSolid();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleLightMode();
        }
    }

    void ToggleLightMode()
    {
        isFlashing = !isFlashing;

        if (isFlashing)
        {
            SetRedFlashing();
        }
        else
        {
            SetWhiteSolid();
        }
    }

    public void SetWhiteSolid()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        pointLight.color = Color.white;
        pointLight.enabled = false;
    }

    public void SetRedFlashing()
    {
        pointLight.enabled = true;
        pointLight.color = Color.red;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            pointLight.enabled = !pointLight.enabled;
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
