using UnityEngine;
using Valve.VR.InteractionSystem;

public class HelmetSnap_DW : MonoBehaviour
{
    [Header("References")]
    public Transform followHead;
    public GameObject tintObject;

    [Header("Snap Settings")]
    public float snapDistance = 0.5f;

    [Header("Worn Offset")]
    public Vector3 wornLocalPosition;
    public Vector3 wornLocalEuler;

    private Rigidbody rb;
    private Interactable interactable;
    private MeshRenderer[] renderers;

    private bool isWorn = false;
    private bool wasHeld = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
        renderers = GetComponentsInChildren<MeshRenderer>(true);
    }

    void Update()
    {
        if (interactable == null) return;

        bool isHeld = interactable.attachedToHand != null;

        // 🔥 RELEASE DETECTED
        if (wasHeld && !isHeld)
        {
            float dist = Vector3.Distance(transform.position, followHead.position);

            if (dist <= snapDistance)
            {
                Equip();
            }
        }

        // 🔥 GRABBED WHILE WORN → REMOVE
        if (isWorn && isHeld)
        {
            Remove();
        }

        wasHeld = isHeld;
    }

    void Equip()
    {
        isWorn = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(followHead);
        transform.localPosition = wornLocalPosition;
        transform.localRotation = Quaternion.Euler(wornLocalEuler);

        SetVisible(false);

        if (tintObject != null)
            tintObject.SetActive(true);
    }

    void Remove()
    {
        isWorn = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        SetVisible(true);

        if (tintObject != null)
            tintObject.SetActive(false);
    }

    void SetVisible(bool visible)
    {
        foreach (var r in renderers)
            r.enabled = visible;
    }
}
