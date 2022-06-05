using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerIdleState(PlayerStateManager psm) : base(psm) {
        player = psm;
    }
    public override void EnterState() {
        player.isIdle = true;
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        player.isIdle = false;
    }

    public override void CheckStateUpdate() {
        if(player.isJumpPressed && player.canJump) {
            SwitchState(player.JumpingState);
        }

        if(player.isMovePressed) {
            SwitchState(player.WalkingState);
        }

        if(!player.characterController.isGrounded) {
            SwitchState(player.FallingState);
        }
    }
}