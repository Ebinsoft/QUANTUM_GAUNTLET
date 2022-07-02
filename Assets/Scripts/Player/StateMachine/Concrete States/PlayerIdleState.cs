using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerIdleState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
    }
    public override void EnterState()
    {
        player.isIdle = true;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isIdle = false;
    }

    public override void CheckStateUpdate()
    {
        if (player.jumpsLeft > 0 && player.isJumpTriggered)
        {
            SwitchState(player.JumpingState);
        }

        else if (player.isUtilityAttackTriggered)
        {
            SwitchState(player.DashingState);
        }

        else if (player.isMovePressed)
        {
            SwitchState(player.WalkingState);
        }

        else if (!player.characterController.isGrounded)
        {
            SwitchState(player.FallingState);
        }

        else if (player.isSpecial1Triggered && player.EnoughManaFor(MoveType.Special1))
        {
            SwitchState(player.Special1State);
        }

        else if (player.isSpecial2Triggered && player.EnoughManaFor(MoveType.Special2))
        {
            SwitchState(player.Special2State);
        }

        else if (player.isSpecial3Triggered && player.EnoughManaFor(MoveType.Special3))
        {
            SwitchState(player.Special3State);
        }

        else if (player.isHeavyAttackTriggered)
        {
            SwitchState(player.HeavyAttackState);
        }

        else if (player.isLightAttackTriggered)
        {
            SwitchState(player.LightAttackState);
        }
    }
}