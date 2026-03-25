using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBlob_DW : MonoBehaviour
{

    public Animator blobAnimator;   // Animator on the empty orbit object
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Optional: ignore hitting the player
        if (collision.gameObject.CompareTag("BlobCollider"))
        {
            if (blobAnimator != null)
                blobAnimator.SetTrigger("BlobGrow");
            Debug.Log("hit blob collider");

            Destroy(collision.gameObject);

        }
        else
        {
            Debug.Log("DID NOT hit blob collider");
           // Destroy(gameObject);

        }
 
      
    }

}
