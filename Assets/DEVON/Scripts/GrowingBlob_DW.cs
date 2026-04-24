using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBlob_DW : MonoBehaviour
{
    public enum BlobType
    {
        FrontWing,
        BackWing,
        PowerBank
    }

    public Animator blobAnimator;
    public EventController eventController;
    // Animator on the empty orbit object
    // Start is called before the first frame update
    public BlobType blobType;
    public Rigidbody rb;
    void Start()
    {
        eventController = FindObjectOfType<EventController>();
        Debug.Log("Blobbing found event controller: " + eventController.name);
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
            blobAnimator = collision.gameObject.GetComponentInChildren<Animator>();
            rb = collision.gameObject.GetComponent<Rigidbody>();
            if (blobAnimator != null)
            {
                rb.isKinematic = true; // Make the blob stop moving
                if (blobType == BlobType.FrontWing)
                {
                    blobAnimator.SetTrigger("FrontWingGrow");
                    eventController.frontWingBlobbed = true;
                }
                else if (blobType == BlobType.BackWing)
                {
                    blobAnimator.SetTrigger("BackWingGrow");
                    eventController.backWingBlobbed = true;
                }
                else if (blobType == BlobType.PowerBank)
                {
                    blobAnimator.SetTrigger("PowerBankGrow");
                    eventController.powerBankBlobbed = true;
                }
            }
            Debug.Log("hit blob collider");

            //Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log("DID NOT hit blob collider");
           // Destroy(gameObject);

        }
 
      
    }

}
