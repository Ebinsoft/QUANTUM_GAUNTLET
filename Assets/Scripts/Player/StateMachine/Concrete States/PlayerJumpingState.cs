using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    
    private PlayerStateManager player;
    public PlayerJumpingState(PlayerStateManager psm) : base(psm){
        player = psm;
        canMove = true;
    }
    public override void EnterState() {
        Jump();
    }

    public override void UpdateState() {
        if(player.canJump) {
            Jump();
        }
    }

    public override void ExitState() {
        player.isJumping = false;
        player.anim.SetBool("Jumping", false);
    }

    public override void CheckStateUpdate() {
        if(player.characterController.isGrounded) {
            SwitchState(player.IdleState);
        }

        if(player.playerManager.currentMovement.y <= 0.0f) {
            SwitchState(player.FallingState);
        }
    }

    private void Jump() {
        if (player.jumpsLeft > 0 && player.canJump) {
            // so we can't hold jump to keep jumping
            player.playerManager.currentMovement.y = player.initialJumpVelocity;
            player.jumpsLeft--;
            player.isJumping = true;
            player.anim.SetBool("Jumping", true);
            player.canJump = false;
        }
    }
}
