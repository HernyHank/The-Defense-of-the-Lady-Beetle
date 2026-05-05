using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReParent : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform targetParent;
    public Transform movableTorpedo;
    public void ReParentTorpedo()
    {
        StartCoroutine(ExecuteReparent());
    }

    private IEnumerator ExecuteReparent()
    {
        // Wait until the very end of the frame so the Animator is finished
        yield return new WaitForSeconds(0.1f);

        movableTorpedo.SetParent(targetParent);
        Debug.Log("Torpedo tried to set parent to: " + targetParent.name);
        // Optional: Reset position if it needs to snap to the parent's center
        /*movableTorpedo.localPosition = Vector3.zero;
        movableTorpedo.localRotation = Quaternion.identity;*/
    }
}

