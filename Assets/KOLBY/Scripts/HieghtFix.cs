using UnityEngine;

public class VRCharacterControllerSync : MonoBehaviour
{
    public CharacterController controller;
    public Transform head;

    void Update()
    {
        controller.height = head.localPosition.y;

        Vector3 center = Vector3.zero;
        center.y = controller.height / 2;
        center.x = head.localPosition.x;
        center.z = head.localPosition.z;

        controller.center = center;
    }
}