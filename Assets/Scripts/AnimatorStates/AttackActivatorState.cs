using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActivatorState : StateMachineBehaviour
{
    public AttackInfo attack;
    public MoveType moveType;

    private PlayerAttackHandler handler;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (handler == null)
        {
            handler = animator.transform.root.gameObject.GetComponent<PlayerAttackHandler>();
        }

        handler.InitiateAttack(attack);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handler.FinishAttack();
    }
}
