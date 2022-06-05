using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerFallingState(PlayerStateManager psm) : base(psm) {
        player = psm;
        canMove = true;
    }
    public override void EnterState() {
        player.isFalling = true;
        player.gravityMultiplier = player.fallMultiplier;

    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        player.isFalling = false;
        player.gravityMultiplier = 1.0f;
    }

    public override void CheckStateUpdate() {
        if(player.characterController.isGrounded) {
            SwitchState(player.IdleState);
        }

        if(player.isJumpPressed && player.canJump) {
            SwitchState(player.JumpingState);
        }
    }
}
