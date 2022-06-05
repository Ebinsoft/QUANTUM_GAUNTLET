using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    
    private PlayerStateManager player;
    public PlayerJumpingState(PlayerStateManager psm) : base(psm){
        player = psm;
        canMove = true;
    }
    public override void EnterState() {
        Debug.Log("jump state");
        Debug.Log(canMove);
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
    }

    private void Jump() {
        if (player.jumpsLeft > 0 && player.isJumpPressed && player.canJump) {
            // so we can't hold jump to keep jumping
            player.canJump = false;
            player.playerManager.currentMovement.y = player.initialJumpVelocity;
            player.jumpsLeft--;
            player.isJumping = true;
            player.anim.SetBool("Jumping", true);
        }
    }
}
