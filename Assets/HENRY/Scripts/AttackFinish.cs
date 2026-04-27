using UnityEngine;

public class AttackFinish : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. Get the parent animator
        Animator orbitAnimator = animator.transform.parent.GetComponentInParent<Animator>();

        if (orbitAnimator != null)
        {
            orbitAnimator.SetTrigger("StartOrbit");
        } else
        {
            Debug.Log("Did not find orbit animator");
        }

        animator.SetTrigger("StartFly");

        PirateShip_HM pirateScript = orbitAnimator.GetComponent<PirateShip_HM>();

        // 2. Tell the PirateShip script to handle the delay and the next attack
        if (pirateScript != null)
        {
            // We tell the script to start its own internal timer
            pirateScript.PrepareNextAttack(2f);
        }
        else
        {
            Debug.Log("Did not find pirate screipt");
        }
    }
}
