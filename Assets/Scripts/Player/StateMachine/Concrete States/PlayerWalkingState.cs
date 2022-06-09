using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerWalkingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
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

        else if (player.jumpsLeft > 0 && player.jumpTriggered)
        {
            SwitchState(player.JumpingState);
        }

        else if (!player.characterController.isGrounded)
        {
            SwitchState(player.FallingState);
        }

    }

}
