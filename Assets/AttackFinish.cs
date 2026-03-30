using UnityEngine;

public class AttackFinish : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. Get the parent animator
        Animator orbitAnimator = animator.transform.GetComponentInParent<Animator>();

        if (orbitAnimator != null)
        {
            orbitAnimator.SetTrigger("StartOrbit");
        }

        animator.SetTrigger("StartFly");

        // 2. Tell the PirateShip script to handle the delay and the next attack
        PirateShip_HM pirateScript = animator.GetComponent<PirateShip_HM>();
        if (pirateScript != null)
        {
            // We tell the script to start its own internal timer
            pirateScript.PrepareNextAttack(2f);
        }
    }
}
