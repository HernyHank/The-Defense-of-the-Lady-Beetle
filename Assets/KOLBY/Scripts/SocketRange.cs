using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections;

public class BatterySnapZone : MonoBehaviour
{
    public Transform snapPoint;
    private Interactable currentBattery;

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentBattery = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentBattery != null && other.gameObject == currentBattery.gameObject)
        {
            currentBattery = null;
        }
    }

    public void TrySnap()
    {
        if (currentBattery != null)
        {
            StartCoroutine(SnapToPosition(currentBattery.transform));
        }
    }
    IEnumerator SnapToPosition(Transform obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        float t = 0f;
        float speed = 5f;

        Vector3 startPos = obj.position;
        Quaternion startRot = obj.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            obj.position = Vector3.Lerp(startPos, snapPoint.position, t);
            obj.rotation = Quaternion.Slerp(startRot, snapPoint.rotation, t);

            yield return null;
        }

        obj.position = snapPoint.position;
        obj.rotation = snapPoint.rotation;
    }
}