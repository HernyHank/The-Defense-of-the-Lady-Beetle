using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections;

public class BatterySnapZone : MonoBehaviour
{
    public Transform snapPoint;

    public enum SnapZoneType
    {
        Generator,
        Trash
    }

    public SnapZoneType zoneType;

    public Animator doorAnimator;          // Only used for Trash
    public Transform ejectDirection;       // Empty GameObject pointing outward
    public float ejectForce = 5f;

    private Interactable currentBattery;

    public bool IsBatteryInZone(GameObject obj)
    {
        return currentBattery != null && currentBattery.gameObject == obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        BatterySnap battery = other.GetComponent<BatterySnap>();
        if (battery != null)
        {
            currentBattery = battery.GetComponent<Interactable>();
            battery.SetCurrentZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BatterySnap battery = other.GetComponent<BatterySnap>();

        if (battery != null)
        {
            battery.ClearZone(this);

            if (currentBattery != null && other.gameObject == currentBattery.gameObject)
            {
                currentBattery = null;
            }
        }
    }

    public void TrySnap(BatterySnap battery)
    {
        if (currentBattery != null)
        {
            StartCoroutine(SnapToPosition(battery));
        }
    }

    IEnumerator SnapToPosition(BatterySnap battery)
    {
        Transform obj = battery.transform;
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        float t = 0f;
        float speed = 1f;

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

        // ?? SNAP FINISHED ? decide what happens
        OnBatterySnapped(battery);
    }

    void OnBatterySnapped(BatterySnap battery)
    {
        if (zoneType == SnapZoneType.Generator)
        {
            HandleGenerator(battery);
        }
        else if (zoneType == SnapZoneType.Trash)
        {
            HandleTrash(battery);
        }
    }

    void HandleGenerator(BatterySnap battery)
    {
        if (battery.currentState == BatterySnap.BatteryState.Full)
        {
            Debug.Log("Generator powered!");
            // TODO: Stop alarm here
        }
        else
        {
            Debug.Log("Empty battery - still broken.");
            // TODO: Keep alarm going
        }
    }

    void HandleTrash(BatterySnap battery)
    {
        if (battery.currentState == BatterySnap.BatteryState.Empty)
        {
            StartCoroutine(DisposeBattery(battery));
        }
        else
        {
            Debug.Log("Full battery inserted into trash - doing nothing.");
        }
    }

    IEnumerator DisposeBattery(BatterySnap battery)
    {
        battery.SetInteractable(false);

        // Close door
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("ChuteClose");
        }

        yield return new WaitForSeconds(1f);

        Rigidbody rb = battery.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;

        if (ejectDirection != null)
        {
            rb.AddForce(ejectDirection.forward * ejectForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.5f);

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("ChuteOpen");
        }

        yield return new WaitForSeconds(4f);

        Destroy(battery.gameObject);
    }
}