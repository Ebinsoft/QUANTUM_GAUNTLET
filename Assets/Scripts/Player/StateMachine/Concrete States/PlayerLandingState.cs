using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private PlayerManager player;

    // time to stay in landing state before going back to idle
    private float cooldownBeforeIdle = 0.2f;

    // time in state before starting a new jump
    private float cooldownBeforeJump = 0.1f;

    // how long we've been in the state so far
    private float timer = 0;

    public PlayerLandingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        timer = 0;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
    }

    public override void ExitState()
    {
    }

    public override void CheckStateUpdate()
    {
        if (timer >= cooldownBeforeIdle)
        {
            SwitchState(player.IdleState);
        }

        else if (timer >= cooldownBeforeJump && player.jumpsLeft > 0 && player.isJumpTriggered)
        {
            SwitchState(player.JumpingState);
        }
    }
}