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
    public Transform blobSnapTransform;
    [Tooltip("Time (seconds) the goo will take to move to the snap point")]
    public float snapDuration = 0.5f;

    // Animator on the empty orbit object
    // Start is called before the first frame update
    public BlobType blobType;
    public Rigidbody rb;

    void Start()
    {
        eventController = FindObjectOfType<EventController>();
        if (eventController != null)
            Debug.Log("Blobbing found event controller: " + eventController.name);
        else
            Debug.LogWarning("GrowingBlob_DW: EventController not found in scene.");
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

            // start the snap coroutine and pass the collided object references
            StartCoroutine(snapToBlobPoint(collision.gameObject, rb, blobAnimator));
        }
    }

    IEnumerator snapToBlobPoint(GameObject blobObject, Rigidbody blobRb, Animator blobAnim)
    {
        if (blobObject == null)
        {
            yield break;
        }

        // If no target snap transform specified, behave as before but still fire animator/flags
        if (blobSnapTransform == null)
        {
            Debug.LogWarning("GrowingBlob_DW: blobSnapTransform is not set. Triggering animator and flags without moving.");
            if (blobAnim != null)
            {
                TriggerBlobAnimationAndFlag(blobAnim);
            }
            yield break;
        }

        // disable physics interactions while moving
        if (blobRb != null)
        {
            blobRb.isKinematic = true;
            blobRb.velocity = Vector3.zero;
            blobRb.angularVelocity = Vector3.zero;
            Collider c = blobObject.GetComponent<Collider>();
            if (c != null) c.enabled = false;
        }

        Transform t = blobObject.transform;
        Vector3 startPos = t.position;
        Quaternion startRot = t.rotation;
        Vector3 targetPos = blobSnapTransform.position;
        Quaternion targetRot = blobSnapTransform.rotation;

        float elapsed = 0f;
        float duration = Mathf.Max(0.0001f, snapDuration); // avoid division by zero

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);

            // smooth step easing for nicer motion
            float ease = Mathf.SmoothStep(0f, 1f, alpha);

            t.position = Vector3.Lerp(startPos, targetPos, ease);
            t.rotation = Quaternion.Slerp(startRot, targetRot, ease);

            yield return null;
        }

        // final snap to exact target
        t.position = targetPos;
        t.rotation = targetRot;

        // parent the blob to the snap transform so it follows the wing/bank
        t.SetParent(blobSnapTransform, true);

        // trigger animator and set event flags
        if (blobAnim != null)
        {
            TriggerBlobAnimationAndFlag(blobAnim);
        }

        // keep rigidbody kinematic and collider disabled while attached
        if (blobRb != null)
        {
            blobRb.isKinematic = true;
            Collider c2 = blobObject.GetComponent<Collider>();
            if (c2 != null) c2.enabled = false;
        }

        Debug.Log("hit blob collider and snapped");
    }

    private void TriggerBlobAnimationAndFlag(Animator blobAnim)
    {
        switch (blobType)
        {
            case BlobType.FrontWing:
                blobAnim.SetTrigger("FrontWingGrow");
                if (eventController != null) eventController.frontWingBlobbed = true;
                break;
            case BlobType.BackWing:
                blobAnim.SetTrigger("BackWingGrow");
                if (eventController != null) eventController.backWingBlobbed = true;
                break;
            case BlobType.PowerBank:
                blobAnim.SetTrigger("PowerBankGrow");
                if (eventController != null) eventController.powerBankBlobbed = true;
                break;
        }
    }
}

