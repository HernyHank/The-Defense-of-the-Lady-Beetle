using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLower_MS : MonoBehaviour
{
    public float ShieldHealth_MS = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator LowerVariable()
    {
        while(ShieldHealth_MS >= 1)
        {
            ShieldHealth_MS -= 1;
            //yield return new WaitForSeconds(2);
        }
        return LowerVariable();
    }

    // Update is called once per frame
    void Update()
    {
        if(ShieldHealth_MS % 5 == 0){
            Debug.Log(ShieldHealth_MS);
        }
            
    }
}
