using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActivatorStateMachine : StateMachineBehaviour
{
    public AttackInfo attack;
    public MoveType moveType;

    private PlayerAttackHandler handler;

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (handler == null)
        {
            handler = animator.transform.root.gameObject.GetComponent<PlayerAttackHandler>();
        }

        handler.InitiateAttack(attack);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        handler.FinishAttack();
    }
}
