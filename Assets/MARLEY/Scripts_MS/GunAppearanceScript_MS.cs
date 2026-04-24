using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.ParticleSystem;

public class GunAppearanceScript_MS : MonoBehaviour
{
    public float scaleTime = 1f; // time to fully grow/shrink

    private Vector3 targetScale;
    private Vector3 shrinkTarget;
    private float fixedY;
    private bool isGrowing = false;
    private bool isShrinking = false;

    public ParticleSystem particles; //assign in inspector


    void Start()
    {
        fixedY = transform.localScale.y;

        // Target = full size on X/Z, original Y
        targetScale = new Vector3(1f, fixedY, 1f);

        // Start at zero scale on X/Z, keep Y
        transform.localScale = new Vector3(0f, fixedY, 0f);
    }

    void Update()
    {
        Debug.Log("UPDATE RUNNING - scale: " + transform.localScale);
        if (Input.GetKeyDown(KeyCode.K))
        {
            isGrowing = true;
            isShrinking = false;
            if (particles != null)
            {
                particles.Play();
            }
            //StartCoroutine(DisableAfterParticles());

        }

        Vector3 current = transform.localScale;
        Vector3 shrinkTarget = new Vector3(0f, fixedY, 0f);


        // Calculate speed so it completes in exactly scaleTime
        float speed = 1f / scaleTime;

        float newX = current.x;
        float newZ = current.z;

        if (isGrowing)
        {
            newX = Mathf.MoveTowards(current.x, targetScale.x, speed * Time.deltaTime);
            newZ = Mathf.MoveTowards(current.z, targetScale.z, speed * Time.deltaTime);
        }
        else if (isShrinking)
        {
            newX = Mathf.MoveTowards(current.x, shrinkTarget.x, speed * Time.deltaTime);
            newZ = Mathf.MoveTowards(current.z, shrinkTarget.z, speed * Time.deltaTime);
        }

        transform.localScale = new Vector3(newX, fixedY, newZ);

        if (isGrowing &&
             Mathf.Approximately(newX, targetScale.x) &&
             Mathf.Approximately(newZ, targetScale.z))
        {
            isGrowing = false;
            isShrinking = true;
        }

        if (isShrinking &&
            Mathf.Approximately(newX, shrinkTarget.x) &&
            Mathf.Approximately(newZ, shrinkTarget.z))
        {
            isShrinking = false;
        }

        /*float newX = Mathf.MoveTowards(current.x, isGrowing ? targetScale.x : shrinkTarget.x, speed * Time.deltaTime);
        float newZ = Mathf.MoveTowards(current.z, isGrowing ? targetScale.z : shrinkTarget.z, speed * Time.deltaTime);

        transform.localScale = new Vector3(newX, fixedY, newZ);

        if (isGrowing &&
            Mathf.Approximately(newX, targetScale.x) &&
            Mathf.Approximately(newZ, targetScale.z))
        {
            isGrowing = false;
            isShrinking = true;
        }

        if (isShrinking &&
            Mathf.Approximately(newX, shrinkTarget.x) &&
            Mathf.Approximately(newZ, shrinkTarget.z))
        {
            isShrinking = false;
        }*/
    }
    /*IEnumerator DisableAfterParticles()
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
    }*/
}
