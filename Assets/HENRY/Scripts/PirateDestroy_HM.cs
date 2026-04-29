using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PirateDestroy_HM : MonoBehaviour
{
    private int pirateShotLayer;
    public EventController controller;
    Animator orbitAnimator;
    Animator bodyAnimator;

    [Header("VFX")]
    public GameObject explosionPrefab;

    // Prevent multiple explosions from the same pirate instance
    private bool hasBeenDestroyed = false;

    private void Awake()
    {
        controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        orbitAnimator = this.gameObject.transform.parent.GetComponent<Animator>();
        bodyAnimator = this.gameObject.GetComponent<Animator>();
    }

    int explosionCount = 0; // For debugging

    void OnTriggerStay(Collider other)
    {
        // Guard: one-shot only

        // Only trigger once when conditions met
        if (other.CompareTag("Gun") && controller != null && controller.turretCanShoot && JoystickManager.Instance.button2)
        {
            explosionCount++;
            

            // Spawn explosion at this object's current position (last known coordinates)
            SpawnExplosionAt(transform.position);
            Debug.Log("collPirate ship is destroyed! Explosion count" + explosionCount);

            orbitAnimator.Rebind();
            orbitAnimator.Update(0f);
            bodyAnimator.Rebind();
            bodyAnimator.Update(0f);

            getParentAndSend();

            // Hide/disable the ship
            this.gameObject.SetActive(false);
        }

        return;
    }

    private void SpawnExplosionAt(Vector3 position)
    {
        if (explosionPrefab == null)
        {
            Debug.LogWarning("PirateDestroy_HM: explosionPrefab is not assigned.");
            return;
        }

        GameObject inst = Instantiate(explosionPrefab, position, Quaternion.identity);
        StartCoroutine(AutoDestroyExplosion(inst));
    }

    private IEnumerator AutoDestroyExplosion(GameObject explosion)
    {
        if (explosion == null) yield break;

        // Try to compute a safe lifetime from any child ParticleSystem(s)
        float waitTime = 5f; // fallback

        ParticleSystem ps = explosion.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            float duration = main.duration;
            float startLifetime = 0f;

            // Handle MinMaxCurve safely
            try
            {
                if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants)
                    startLifetime = main.startLifetime.constantMax;
                else
                    startLifetime = main.startLifetime.constant;
            }
            catch
            {
                // fall back to a small value if anything unexpected happens
                startLifetime = 1f;
            }

            waitTime = duration + startLifetime + 0.5f;
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(explosion);
    }

    public void getParentAndSend()
    {

        orbitAnimator.Rebind();
        orbitAnimator.Update(0f);
        bodyAnimator.Rebind();
        bodyAnimator.Update(0f);
        // Safely walk up the hierarchy with a hard limit to avoid infinite loops
        Transform currentParent = transform;
        const int maxLevelsUp = 10;
        int levels = 0;

        // Look for an ancestor whose name matches "CameraJoint (N)"
        Regex cameraJointRegex = new Regex(@"^CameraJoint\s*\((\d+)\)$");

        while (currentParent != null && levels < maxLevelsUp)
        {
            Match m = cameraJointRegex.Match(currentParent.name);
            if (m.Success)
            {
                if (int.TryParse(m.Groups[1].Value, out int index))
                {
                    //Debug.Log("sending destroy ship of index " + index);
                    controller?.DestroyShip(index);
                    return;
                }

                Debug.LogWarning($"getParentAndSend: failed to parse index from CameraJoint match in '{currentParent.name}'");
                break;
            }

            currentParent = currentParent.parent;
            levels++;
        }

        // Fallback: if no exact match found, try to extract the first integer anywhere in the final ancestor name
        if (currentParent != null)
        {
            string targetName = currentParent.name;
            Debug.Log($"getParentAndSend fallback using name='{targetName}'");

            Match fallbackMatch = Regex.Match(targetName, @"\d+");
            if (fallbackMatch.Success && int.TryParse(fallbackMatch.Value, out int fallbackIndex))
            {
                Debug.Log("sending destroy ship of index (fallback) " + fallbackIndex);
                controller?.DestroyShip(fallbackIndex);
                return;
            }

            Debug.LogWarning($"getParentAndSend: could not parse index from '{targetName}'. No numeric token found.");
            return;
        }

        // If we exhausted the ancestry without finding a usable name
        Debug.LogWarning("getParentAndSend: reached top of hierarchy without finding a CameraJoint-like ancestor.");
    }

}