using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLower_MS : MonoBehaviour
{
    public int ShieldHealth_MS;
    public float YScale_MS;
    public GameObject PivotPoint;

    void Start()
    {
        ShieldHealth_MS = 100;
        YScale_MS = 1.0f;
        StartCoroutine(LowerVariable());
        PivotPoint = GameObject.Find("PivotPoint");
    }

    void Update()
    {
        YScale_MS = ShieldHealth_MS / 100f;
        PivotPoint.transform.localScale = new Vector3(1f, YScale_MS, 1f);
        if (ShieldHealth_MS <= 0)
        {
            ShieldHealth_MS = 0;
            Debug.Log("Shield gone, you're gonna freaking die");
        }

        if(ShieldHealth_MS <= 95 && Input.GetKeyDown(KeyCode.F))
        {
            ShieldHealth_MS = 100;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ShieldHealth_MS = ShieldHealth_MS / 2 - ShieldHealth_MS % 1;
        }

        
    }

    public IEnumerator LowerVariable()
    {
        while (ShieldHealth_MS >= 5)
        {
            ShieldHealth_MS -= 5;
            yield return new WaitForSeconds(5);
            Debug.Log(ShieldHealth_MS);
        }
    }
}
