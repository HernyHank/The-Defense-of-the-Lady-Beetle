using UnityEngine;
using System.Diagnostics;

[DisallowMultipleComponent]
public class TransformChangeLogger : MonoBehaviour
{
    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform lastParent;

    void Start()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
        lastParent = transform.parent;
    }

    void LateUpdate()
    {
        if (transform.position != lastPos || transform.rotation != lastRot || transform.parent != lastParent)
        {
            UnityEngine.Debug.Log($"[TransformChangeLogger] Frame {Time.frameCount} - pos {transform.position} rot {transform.rotation.eulerAngles} parent {(transform.parent ? transform.parent.name : "null")}");
            // capture stack to help find the call (expensive; remove after debugging)
            var st = new StackTrace(true);
            UnityEngine.Debug.Log($"Stack trace:\n{st}");
            lastPos = transform.position;
            lastRot = transform.rotation;
            lastParent = transform.parent;
        }
    }
}