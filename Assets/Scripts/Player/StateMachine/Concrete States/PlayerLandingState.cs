using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private PlayerStateManager player;

    // time to stay in landing state before going back to idle
    private float cooldownBeforeIdle = 0.2f;

    // time in state before starting a new jump
    private float cooldownBeforeJump = 0.1f;

    // how long we've been in the state so far
    private float timer = 0;

    public PlayerLandingState(PlayerStateManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
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

        if (timer >= cooldownBeforeJump && player.jumpsLeft > 0 && player.canJump)
        {
            SwitchState(player.JumpingState);
        }
    }
}