using UnityEngine;

public class PlayerHeavyAttackState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerHeavyAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
    }

    public override void EnterState()
    {
        player.isHeavyAttackTriggered = false;
        player.isAttacking = true;
        player.anim.SetTrigger("HeavyAttack");

        player.heavyAttacksLeft--;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isAttacking = false;

        // this will usually be redundant except for cases where an attack is interrupted
        player.gameObject.GetComponent<PlayerAttackHandler>().FinishAttack();
    }

    public override void CheckStateUpdate()
    {
        // InMelee stays true until animator leaves the melee sub-state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
            player.heavyAttacksLeft = player.maxHeavyAttackChain;
        }

        else if (player.heavyAttacksLeft > 0 && player.isHeavyAttackTriggered)
        {
            SwitchState(player.HeavyAttackState);
        }
    }
}
