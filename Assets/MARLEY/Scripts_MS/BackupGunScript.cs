using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static UnityEngine.ParticleSystem;

public class BackupGunScript : MonoBehaviour
{
    public float scaleTime = 0.005f;

    private Vector3 targetScale;
    private float fixedY;
    private bool isGrowing = false;
    public ParticleSystem particles;

    private bool previousButtonState = false;
    public bool isFiring { get; private set; }
    public float cooldown = 0.5f;
    private float lastShotTime;

    Animator turretAnimator;
    void Start()
    {
        fixedY = transform.localScale.y;

        //Target = full size on X/Z, original Y
        targetScale = new Vector3(1f, fixedY, 1f);

        //Start at zero scale on X/Z, keep Y
        transform.localScale = new Vector3(0f, fixedY, 0f);

        Transform sibling = transform.parent.Find("turret");
        turretAnimator = sibling.gameObject.GetComponent<Animator>();
        if(turretAnimator == null)
        {
            Debug.LogWarning("BackupGunScript: No Animator found on sibling 'turret' object.");
        }
        else
        {
          // Debug.Log("Animator found on " + sibling.gameObject.name);
        }   
    }

    bool blastIsAfoot = false;
    void Update()
    {
        bool currentButton = JoystickManager.Instance.button2;

        bool buttonPressed = currentButton && !previousButtonState;

        if (buttonPressed && Time.time >= lastShotTime + cooldown && !blastIsAfoot)
        {
            Fire();
        }

        previousButtonState = currentButton;

        //if (Input.GetKeyDown(KeyCode.K))

        Vector3 current = transform.localScale;

        Vector3 shrinkTarget = new Vector3(0f, fixedY, 0f);

        //Calculate speed so it completes in exactly scaleTime
        float speed = 1f / scaleTime;

        //Move only X and Z, keep Y fixed
        float newX = Mathf.MoveTowards(current.x, isGrowing ? targetScale.x : shrinkTarget.x, speed * Time.deltaTime);
        float newZ = Mathf.MoveTowards(current.z, isGrowing ? targetScale.z : shrinkTarget.z, speed * Time.deltaTime);

        transform.localScale = new Vector3(newX, fixedY, newZ);

        if (isGrowing && Mathf.Approximately(newX, targetScale.x) && Mathf.Approximately(newZ, targetScale.z))
            {
                isGrowing = false;
                blastIsAfoot = false;
            }
    }

    void Fire()
    {
        lastShotTime = Time.time;
        isFiring = true;
        blastIsAfoot = true;
        isGrowing = true;
        if (turretAnimator != null)
        {
            turretAnimator.SetTrigger("turretShoot");
        }
        if (particles != null)
        {
            particles.Play();
        }
        StartCoroutine(DisableAfterParticles());

       /* gunRenderer.enabled = true;
        Invoke(nameof(StopFiring), 0.1f); */// how long the cylinder shows
    }

/*    void StopFiring()
    {
        gunRenderer.enabled = false;
    }*/

    IEnumerator DisableAfterParticles()
    {
        if (particles != null)
        {
            yield return new WaitForSeconds(particles.main.duration);
        }
        else
        {
            yield return null;
        }
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);

    }
}