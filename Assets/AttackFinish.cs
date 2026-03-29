using UnityEngine;

public class AttackFinish : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. Find the specific manager object in the scene


        // 2. Get the Animator component from that object
        Animator orbitAnimator = animator.transform.parent.GetComponentInParent<Animator>();
        Debug.Log(animator.gameObject);
            if (orbitAnimator != null)
            {
                Debug.Log("Animator found");
                orbitAnimator.SetTrigger("StartOrbit");
            } else
        {
            Debug.Log("Animator NOT found");
        }

            animator.SetTrigger("StartFly");
    }
}