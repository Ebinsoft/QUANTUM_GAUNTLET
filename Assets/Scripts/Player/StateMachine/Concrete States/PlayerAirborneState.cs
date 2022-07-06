using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    private PlayerManager player;
    private float minTimeBeforeLanding = 0.2f;
    private float landingTimer;
    public PlayerAirborneState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
        canRotate = true;
        cancelMomentum = true;
    }
    public override void EnterState()
    {
        player.isFalling = true;
        player.anim.SetBool("IsFalling", true);
        player.gravityMultiplier = player.fallMultiplier;
        landingTimer = 0f;
    }

    public override void UpdateState()
    {
        landingTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        player.isFalling = false;
        player.anim.SetBool("IsFalling", false);
        player.gravityMultiplier = 1.0f;
    }

    public override void CheckStateUpdate()
    {
        if (player.isGrounded && landingTimer >= minTimeBeforeLanding)
        {
            SwitchState(player.LandingState);
        }
        else if (player.isGrounded)
        {
            SwitchState(player.IdleState);
        }
        else if (player.jumpsLeft > 0 && player.isJumpTriggered)
        {
            SwitchState(player.JumpingState);
        }
    }
}
