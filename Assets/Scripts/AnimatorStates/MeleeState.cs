using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("InMelee", true);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("InMelee", false);

        // reset any unused attack triggers
        animator.ResetTrigger("LightAttack");
        animator.ResetTrigger("HeavyAttack");
    }
}
