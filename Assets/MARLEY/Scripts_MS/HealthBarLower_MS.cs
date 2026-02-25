using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLower_MS : MonoBehaviour
{
    public int ShieldHealth_MS;

    void Start()
    {
        ShieldHealth_MS = 100;
        StartCoroutine(LowerVariable());
    }

    public IEnumerator LowerVariable()
    {
        while(ShieldHealth_MS >= 5)
            {
                ShieldHealth_MS -= 5;
                yield return new WaitForSeconds(5);
                Debug.Log(ShieldHealth_MS);
            }
        //return LowerVariable();
    }

    void Update()
    {



        if(ShieldHealth_MS <= 95 && Input.GetKeyDown(KeyCode.P))
        {
            ShieldHealth_MS += 5;
        }

    }
}
