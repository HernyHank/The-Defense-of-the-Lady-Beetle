using UnityEngine;

public class AttackFinish : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. Find the specific manager object in the scene
        GameObject managerObj = GameObject.Find("ShipAnimatorManager");

        if (managerObj != null)
        {
            // 2. Get the Animator component from that object
            Animator orbitAnimator = managerObj.GetComponent<Animator>();
            if (orbitAnimator != null)
            {
                orbitAnimator.SetTrigger("StartOrbit");
            }

            animator.SetTrigger("StartFly");

        }
        else
        {
            Debug.LogWarning("AttackFinish: Could not find ShipAnimatorManager in the scene.");
        }
    }
}