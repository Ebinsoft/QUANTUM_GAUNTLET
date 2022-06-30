using UnityEngine;

public class PlayerLightAttackState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerLightAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.isAttacking = true;

        TriggerHit();
    }

    public override void UpdateState()
    {
        if (player.lightAttacksLeft > 0 && player.isLightAttackTriggered)
        {
            TriggerHit();
        }
    }

    public override void ExitState()
    {
        player.isAttacking = false;
        player.lightAttacksLeft = player.maxLightAttackChain;
    }

    public override void CheckStateUpdate()
    {
        // InMelee stays true until animator leaves the melee sub-state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
        }

        else if (player.isHeavyAttackTriggered)
        {
            SwitchState(player.HeavyAttackState);
        }
    }

    void TriggerHit()
    {
        if (player.lightAttacksLeft > 0)
        {
            player.isLightAttackTriggered = false;
            player.anim.SetTrigger("LightAttack");     // triggers the start of an attack
            player.lightAttacksLeft--;
        }
    }
}
