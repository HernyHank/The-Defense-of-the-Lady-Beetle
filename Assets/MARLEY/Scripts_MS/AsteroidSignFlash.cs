using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsteroidSignFlash : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public asteroidMonitor monitor;

    public float flashSpeed = 0.5f;

    private bool isFlashing = false;
    private Coroutine flashCoroutine;

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshPro>();
        }

        StopFlashing();
    }

    /* void Update()
     {
         if (OnTriggerStay()) //edit here
         {
             ToggleSignFlash();
         }
     }*/

/*    void OnTriggerEnter(Collider other)
    {
        // Check if the object is on the "Asteroid" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            monitor.QueFlash();
            StartFlashing();
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            monitor.QueFlash();
            StartFlashing();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            StopFlashing();
            monitor.QuitFlash();
        }
    }

    /*void ToggleSignFlash()
    {
        isFlashing = !isFlashing;

        if (isFlashing)
        {
            StartFlashing();
        }
        else
        {
            StopFlashing();
        }
    }*/

    void StopFlashing()
    {

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        isFlashing = false;
        textMeshPro.enabled = false;
    }

    void StartFlashing()
    {
        if (isFlashing) return;
        isFlashing = true;

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
            textMeshPro.enabled = !textMeshPro.enabled; //Change to reference the TMP
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
