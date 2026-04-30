using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TorpedoHoming : MonoBehaviour
{
    EventController controller;
    public Transform target = null;
    public Transform myTarget;
    public float speed = 30f;            // forward speed (m/s)
    public float rotateSpeed = 180f;     // degrees per second
    public float lifetime = 10f;

    Rigidbody rb;

    void Awake()
    {
        controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        rb = GetComponent<Rigidbody>();
/*        rb.useGravity = false;
        rb.isKinematic = false;*/
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        SetTarget(myTarget);
        /*Destroy(gameObject, lifetime);*/
    }

    private bool launchedTenFeet = false;
    void FixedUpdate()
    {
        rb.isKinematic = false;
        if (!launchedTenFeet)
        {
            StartCoroutine(LaunchSequence());
        } else if (target != null)
        {
            // Simple homing: steer toward current target position
            Vector3 dir = (target.position - transform.position).normalized;
            if (dir.sqrMagnitude > 0f)
            {
                Quaternion desired = Quaternion.LookRotation(dir);
                Quaternion newRot = Quaternion.RotateTowards(transform.rotation, desired, rotateSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(newRot);
            }
        }

        // maintain forward velocity
        rb.velocity = transform.forward * speed;
    }

    IEnumerator LaunchSequence()
    {
        // Launch forward for a short distance before homing
        float launchTime = 1f; // time to reach 10 feet at 30 m/s
        float elapsed = 0f;
        while (elapsed < launchTime)
        {
            rb.velocity = transform.forward * speed;
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        launchedTenFeet = true;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: spawn VFX, apply damage if necessary
        if(collision.gameObject.CompareTag("BadGuy"))
        {
            // Example: apply damage to enemy
            // EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();
            // if (health != null)
            // {
            //     health.TakeDamage(damageAmount);
            // }
            controller.torpedoHit = true;
            collision.gameObject.GetComponentInParent<Animator>().SetTrigger("Death");
            Destroy(gameObject);
        }      
    }
}
