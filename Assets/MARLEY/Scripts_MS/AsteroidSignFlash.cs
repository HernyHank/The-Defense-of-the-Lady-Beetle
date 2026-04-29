using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsteroidSignFlash : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public KeyCode toggleKey = KeyCode.F;

    public float flashSpeed = 0.5f; // time between on/off

    private bool isFlashing = false;
    private Coroutine flashCoroutine;

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        StopFlashing();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleSignFlash();
        }
    }

    void ToggleSignFlash()
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
    }

    void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        textMeshPro.enabled = false; //Change to reference the TMP
    }

    void StartFlashing()
    {

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
