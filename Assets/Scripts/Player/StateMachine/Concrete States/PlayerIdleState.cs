using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerIdleState(PlayerManager psm) : base(psm)
    {
        player = psm;
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

        else if (player.heavyAttacksLeft > 0 && player.isHeavyAttackTriggered)
        {
            SwitchState(player.HeavyAttackState);
        }

        else if (player.lightAttacksLeft > 0 && player.isLightAttackTriggered)
        {
            SwitchState(player.LightAttackState);
        }
    }
}