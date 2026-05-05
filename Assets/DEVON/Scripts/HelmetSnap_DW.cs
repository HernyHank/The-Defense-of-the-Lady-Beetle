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

    // cached renderers that belong to the tintObject (if any)
    private MeshRenderer[] tintRenderers;

    private bool isWorn = false;
    private bool wasHeld = false;

    EventController eventController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
        renderers = GetComponentsInChildren<MeshRenderer>(true);
        eventController = FindObjectOfType<EventController>();

        // Cache tint renderers if tintObject assigned
        if (tintObject != null)
        {
            tintRenderers = tintObject.GetComponentsInChildren<MeshRenderer>(true);
            // Ensure tintObject is initially disabled (or whatever default you want)
            // tintObject.SetActive(false);
        }
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
        if (isWorn && isHeld && eventController.currentRoom != "Outside")
        {
            Remove();
        }

        wasHeld = isHeld;
    }

    void Equip()
    {
        isWorn = true;

        eventController.helmetIsOn = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(followHead);
        transform.localPosition = wornLocalPosition;
        transform.localRotation = Quaternion.Euler(wornLocalEuler);

        // Hide helmet meshes but do not disable tint renderers
        SetVisible(false);

        if (tintObject != null)
        {
            tintObject.SetActive(true);

            // Make sure tint renderers are enabled (some shaders respond to GameObject active only)
            if (tintRenderers != null)
            {
                foreach (var r in tintRenderers)
                {
                    if (r != null) r.enabled = true;
                }
            }
        }
    }

    void Remove()
    {
        isWorn = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        SetVisible(true);

        if (tintObject != null)
        {
            // disable tint object and its renderers
            if (tintRenderers != null)
            {
                foreach (var r in tintRenderers)
                {
                    if (r != null) r.enabled = false;
                }
            }

            tintObject.SetActive(false);
        }
    }

    void SetVisible(bool visible)
    {
        // More robust: skip any renderer that belongs to tintObject (or its children)
        foreach (var r in renderers)
        {
            if (r == null) continue;

            // If we have a tintObject reference, skip renderers that are part of it
            if (tintObject != null && r.transform.IsChildOf(tintObject.transform))
            {
                // leave tint renderer alone here (Equip/Remove handles it explicitly)
                continue;
            }

            // Otherwise toggle visibility on the helmet meshes
            r.enabled = visible;
        }
    }
}
