using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torpedoIsPrepped : StateMachineBehaviour
{

    //public Transform targetParent;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventController controller = GameObject.Find("EmptyEventController").GetComponent<EventController>();
        animator.enabled = false;
        //animator.transform.SetParent(targetParent);
        ReParent parentScript = animator.gameObject.GetComponent<ReParent>();
        if(parentScript != null)
        {
            parentScript.ReParentTorpedo();
        } else
        {
            Debug.LogError("ReParent script not found on the torpedo object."); 
        }
            controller.torpedoIsLoaded = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
