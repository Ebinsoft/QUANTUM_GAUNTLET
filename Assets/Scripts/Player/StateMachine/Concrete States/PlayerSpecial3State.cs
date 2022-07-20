using UnityEngine;

public class PlayerSpecial3State : PlayerBaseState
{

    private PlayerManager player;
    public PlayerSpecial3State(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.isSpecial3Triggered = false;
        player.anim.SetBool("InSpecialAttack", true);
        player.anim.SetTrigger("Special3");
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
