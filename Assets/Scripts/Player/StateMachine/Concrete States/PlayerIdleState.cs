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
        if(player.jumpsLeft > 0 && player.canJump) {
            SwitchState(player.JumpingState);
        }

        else if(player.isMovePressed) {
            SwitchState(player.WalkingState);
        }
        
        else if(!player.characterController.isGrounded) {
            SwitchState(player.FallingState);
        }
    }
}