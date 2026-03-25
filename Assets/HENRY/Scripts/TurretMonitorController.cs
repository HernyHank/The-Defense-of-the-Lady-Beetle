using UnityEngine;

public class TurretMonitorController : MonoBehaviour
{
    [Header("Camera 1 Setup")]
    public GameObject cam1Object;
    public TurretCam cam1Script;
    public RenderTexture cam1RT;

    [Header("Camera 2 Setup")]
    public GameObject cam2Object;
    public TurretCam cam2Script;
    public RenderTexture cam2RT;

    [Header("Camera 3 Setup")]
    public GameObject cam3Object;
    public TurretCam cam3Script;
    public RenderTexture cam3RT;

    [Header("Camera 4 Setup")]
    public GameObject cam4Object;
    public TurretCam cam4Script;
    public RenderTexture cam4RT;

    private Renderer monitorRenderer;

    void Start()
    {
        monitorRenderer = GetComponent<Renderer>();
        SwitchToCam1(); // Default start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToCam1();
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToCam2();
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToCam3();
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToCam4();
    }

    public void OnPress(int x)
    {
        Debug.Log(x + "was pressed");
        if(x == 1)
            SwitchToCam1();
        if(x == 2)
            SwitchToCam2();
        if(x==3)
            SwitchToCam3();
        if(x == 4)
            SwitchToCam4();
        
    }

    // --- Switch Functions ---

    public void SwitchToCam1()
    {
        UpdateMonitor(cam1RT);
        SetCamState(1);
    }

    public void SwitchToCam2()
    {
        UpdateMonitor(cam2RT);
        SetCamState(2);
    }

    public void SwitchToCam3()
    {
        UpdateMonitor(cam3RT);
        SetCamState(3);
    }

    public void SwitchToCam4()
    {
        UpdateMonitor(cam4RT);
        SetCamState(4);
    }

    // --- Helper Methods to keep code clean ---

    private void UpdateMonitor(RenderTexture rt)
    {
        monitorRenderer.material.mainTexture = rt;
    }

    private void SetCamState(int activeCam)
    {
        // Enable/Disable Objects
        cam1Object.SetActive(activeCam == 1);
        cam2Object.SetActive(activeCam == 2);
        cam3Object.SetActive(activeCam == 3);
        cam4Object.SetActive(activeCam == 4);

        // Enable/Disable Scripts
        cam1Script.enabled = (activeCam == 1);
        cam2Script.enabled = (activeCam == 2);
        cam3Script.enabled = (activeCam == 3);
        cam4Script.enabled = (activeCam == 4);
    }
}