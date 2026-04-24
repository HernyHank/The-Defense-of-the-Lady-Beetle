using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BackupGunScript : MonoBehaviour
{
    public float scaleTime = 0.005f;

    private Vector3 targetScale;
    private float fixedY;
    private bool isGrowing = false;
    public ParticleSystem particles;

    void Start()
    {
        fixedY = transform.localScale.y;

        //Target = full size on X/Z, original Y
        targetScale = new Vector3(1f, fixedY, 1f);

        //Start at zero scale on X/Z, keep Y
        transform.localScale = new Vector3(0f, fixedY, 0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isGrowing = true;
            if (particles != null)
            {
                particles.Play();
            }
            StartCoroutine(DisableAfterParticles());
        }

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
            }
    }

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