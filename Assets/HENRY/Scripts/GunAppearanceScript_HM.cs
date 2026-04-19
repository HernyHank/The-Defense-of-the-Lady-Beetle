using UnityEngine;

public class GunAppearanceScript_HM : MonoBehaviour
{
    private Renderer gunRenderer;
    EventController controller;

    [Header("Shooting Settings")]
    public float cooldown = 0.5f;
    private float lastShotTime;

    // Optional: public state (other scripts can read this)
    public bool isFiring { get; private set; }

    // For detecting button press (not hold)
    private bool previousButtonState = false;

    void Start()
    {
        controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();

        gunRenderer = GetComponent<Renderer>();
        gunRenderer.enabled = false;
    }

    void Update()
    {
        isFiring = false;

        if (!controller.turretCanShoot) return;

        //JOYSTICK SHOOTy
        bool currentButton = JoystickManager.Instance.button2;

        // Detect "button down" (pressed this frame, not held)
        bool buttonPressed = currentButton && !previousButtonState;

        if (buttonPressed && Time.time >= lastShotTime + cooldown)
        {
            Fire();
        }

        //DEBUG KEYBOARD SHOOTy
        /*        if (Input.GetKeyDown(KeyCode.K))
                {
                    Fire();
                }*/
        previousButtonState = currentButton;
    }

    void Fire()
    {
        lastShotTime = Time.time;
        isFiring = true;

        gunRenderer.enabled = true;
        Invoke(nameof(StopFiring), 0.1f); // how long the cylinder shows
    }

    void StopFiring()
    {
        gunRenderer.enabled = false;
    }
}