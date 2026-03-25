using UnityEngine;

public class TurretMonitorController : MonoBehaviour
{
    [Header("Camera 1 Setup")]
    public GameObject cam1Object;        // The Camera GameObject
    public TurretCam cam1Script;         // The Movement Script on Cam 1
    public RenderTexture cam1RT;         // The Render Texture for Cam 1

    [Header("Camera 2 Setup")]
    public GameObject cam2Object;        // The Camera GameObject
    public TurretCam cam2Script;         // The Movement Script on Cam 2
    public RenderTexture cam2RT;         // The Render Texture for Cam 2

    private Renderer monitorRenderer;

    void Start()
    {
        // Gets the Renderer component on the monitor itself
        monitorRenderer = GetComponent<Renderer>();

        // Start with Cam 1 active, Cam 2 frozen
        SwitchToCam1();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToCam1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToCam2();
        }
    }

    void SwitchToCam1()
    {
        // 1. Swap the Texture on the monitor
        monitorRenderer.material.mainTexture = cam1RT;

        // 2. Enable Cam 1's logic and camera
        cam1Script.enabled = true;
        cam1Object.SetActive(true);

        // 3. Disable Cam 2's logic and camera
        cam2Script.enabled = false;
        cam2Object.SetActive(false);
    }

    void SwitchToCam2()
    {
        // 1. Swap the Texture on the monitor
        monitorRenderer.material.mainTexture = cam2RT;

        // 2. Enable Cam 2's logic and camera
        cam2Script.enabled = true;
        cam2Object.SetActive(true);

        // 3. Disable Cam 1's logic and camera
        cam1Script.enabled = false;
        cam1Object.SetActive(false);
    }
}