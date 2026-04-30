using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidoutZone : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform resetTransform;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = resetTransform.position;
        other.gameObject.transform.rotation = resetTransform.rotation;  
    }
}
