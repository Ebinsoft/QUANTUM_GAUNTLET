using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerWalkingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
        canRotate = true;
    }
    public override void EnterState()
    {
        player.isMoving = true;
        player.anim.SetBool("IsMoving", true);
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isMoving = false;
        player.anim.SetBool("IsMoving", false);
    }

    public override void CheckStateUpdate()
    {
        if (!player.isMovePressed)
        {
            SwitchState(player.IdleState);
        }

        else if (player.jumpsLeft > 0 && player.isJumpTriggered)
        {
            SwitchState(player.JumpingState);
        }

        else if (player.dashesLeft > 0 && player.isUtilityAttackTriggered)
        {
            SwitchState(player.DashingState);
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
