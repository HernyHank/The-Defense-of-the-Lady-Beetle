/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAppearanceScript_MS : MonoBehaviour
{
    private Renderer gunRenderer;

    void Start()
    {
        gunRenderer = GetComponent<Renderer>();
        gunRenderer.enabled = false; // hide at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            gunRenderer.enabled = true;
            //play growing animation
            Debug.Log("gun is shot");
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            gunRenderer.enabled = false;
            //play shrinking animation
        }
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAppearanceScript_MS : MonoBehaviour
{
    public float scaleTime = 0.01f; // time to fully grow/shrink

    private Vector3 targetScale;
    private float fixedY;
    private bool isGrowing = false;

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
        if (Input.GetKeyDown(KeyCode.K))
        {
            isGrowing = true;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            isGrowing = false;
        }

        Vector3 current = transform.localScale;
        Vector3 target = isGrowing
            ? targetScale
            : new Vector3(0f, fixedY, 0f);

        // Calculate speed so it completes in exactly scaleTime
        float speed = 1f / scaleTime;

        // Move only X and Z, keep Y fixed
        float newX = Mathf.MoveTowards(current.x, target.x, speed * Time.deltaTime);
        float newZ = Mathf.MoveTowards(current.z, target.z, speed * Time.deltaTime);

        transform.localScale = new Vector3(newX, fixedY, newZ);
    }
}