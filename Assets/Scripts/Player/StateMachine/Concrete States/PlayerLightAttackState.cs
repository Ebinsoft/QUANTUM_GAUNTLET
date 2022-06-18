using UnityEngine;

public class PlayerLightAttackState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerLightAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
    }

    public override void EnterState()
    {
        player.isLightAttackTriggered = false;
        player.isAttacking = true;
        player.anim.SetTrigger("LightAttack");     // triggers the start of an attack

        player.lightAttacksLeft--;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isAttacking = false;
    }

    public override void CheckStateUpdate()
    {
        // InMelee stays true until animator leaves the melee sub-state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
            player.lightAttacksLeft = player.maxLightAttackChain;
        }

        else if (player.heavyAttacksLeft > 0 && player.isHeavyAttackTriggered)
        {
            SwitchState(player.HeavyAttackState);
            player.lightAttacksLeft = player.maxLightAttackChain;
        }

        else if (player.lightAttacksLeft > 0 && player.isLightAttackTriggered)
        {
            SwitchState(player.LightAttackState);
        }
    }
}
