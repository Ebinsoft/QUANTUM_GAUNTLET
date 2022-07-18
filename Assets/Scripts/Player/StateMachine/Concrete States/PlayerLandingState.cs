using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerLandingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.anim.SetBool("InLanding", true);
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InLanding"))
        {
            SwitchState(player.IdleState);
        }
    }
}