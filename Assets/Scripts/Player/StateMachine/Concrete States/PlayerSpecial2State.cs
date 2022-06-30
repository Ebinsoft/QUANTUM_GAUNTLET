using UnityEngine;

public class PlayerSpecial2State : PlayerBaseState
{

    private PlayerManager player;
    public PlayerSpecial2State(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.isSpecial2Triggered = false;
        player.anim.SetTrigger("Special2");
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
