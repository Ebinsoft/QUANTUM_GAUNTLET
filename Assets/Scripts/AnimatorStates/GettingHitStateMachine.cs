using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingHitStateMachine : StateMachineBehaviour
{
    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool("InHit", true);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool("InHit", false);
    }
}
