using UnityEngine;

public class PlayerSpecial1State : PlayerBaseState
{

    private PlayerManager player;
    public PlayerSpecial1State(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
    }

    public override void EnterState()
    {
        player.isSpecial1Triggered = false;
        player.anim.SetTrigger("Special1");
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InSpecialAttack"))
        {
            SwitchState(player.IdleState);
        }
    }
}
