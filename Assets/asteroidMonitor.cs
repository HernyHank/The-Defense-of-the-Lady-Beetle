using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class asteroidMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    //public MarleysAsteroidScript marleyScript;
    public Material normalMaterial;
    public Material warningMaterial;
    public float flashSpeed = 0.5f; // time between on/off

    public bool normalMatActive = true;

    Renderer myRenderer;
    void Start()
    {
            myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
/*        if(marleyScript.asteroidsDetectedInZone)
        {
            Destroy(gameObject);
        }*/

        if(Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(FlashRoutine());
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            StopCoroutine(FlashRoutine());
            myRenderer.material = normalMaterial;
        }


    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            normalMatActive = !normalMatActive;
            myRenderer.material = normalMatActive ? normalMaterial : warningMaterial;
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
