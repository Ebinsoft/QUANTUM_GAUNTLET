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
        player.isAttacking = true;
        player.anim.SetTrigger("LightAttack");     // triggers the start of an attack
        // player.anim.SetBool("InMelee", true);   // true until animator leaves melee state machine
        player.isLightAttackTriggered = false;
        player.attacksLeft--;
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
        // animator should set IsAttacking to false after exiting melee state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
            player.attacksLeft = player.maxAttackChain;
        }

        else if (player.attacksLeft > 0 && player.isLightAttackTriggered)
        {
            SwitchState(player.LightAttackState);
        }
    }
}
