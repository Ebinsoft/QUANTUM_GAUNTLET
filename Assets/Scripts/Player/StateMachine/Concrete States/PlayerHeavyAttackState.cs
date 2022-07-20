using UnityEngine;

public class PlayerHeavyAttackState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerHeavyAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.isAttacking = true;
        player.anim.SetBool("InMelee", true);

        TriggerHit();
    }

    public override void UpdateState()
    {
        if (player.heavyAttacksLeft > 0 && player.isHeavyAttackTriggered)
        {
            TriggerHit();
        }
    }

    public override void ExitState()
    {
        player.isAttacking = false;
        player.heavyAttacksLeft = player.maxHeavyAttackChain;
    }

    public override void CheckStateUpdate()
    {
        // InMelee stays true until animator leaves the melee sub-state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
        }
    }

    void TriggerHit()
    {
        if (player.heavyAttacksLeft > 0)
        {
            player.isHeavyAttackTriggered = false;
            player.anim.SetTrigger("HeavyAttack");     // triggers the start of an attack
            player.heavyAttacksLeft--;
        }
    }
}
