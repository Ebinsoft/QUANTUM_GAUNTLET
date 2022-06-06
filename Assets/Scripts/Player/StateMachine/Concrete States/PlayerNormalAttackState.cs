using UnityEngine;

public class PlayerNormalAttackState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerNormalAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
    }
    public override void EnterState()
    {
        player.isAttacking = true;
        player.anim.SetTrigger("NormalAttack");     // triggers the start of an attack
        // player.anim.SetBool("InMelee", true);   // true until animator leaves melee state machine
        player.attackTriggered = false;
        player.attacksLeft--;
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
        // animator should set IsAttacking to false after exiting melee state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
            player.attacksLeft = player.maxAttackChain;
        }

        else if (player.attacksLeft > 0 && player.attackTriggered)
        {
            SwitchState(player.NormalAttackState);
        }
    }
}
