using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    
    private PlayerStateManager player;
    public PlayerJumpingState(PlayerStateManager psm) : base(psm){
        player = psm;
        canMove = true;
    }
    public override void EnterState() {
        player.isJumping = true;
        player.anim.SetBool("Jumping", true);
        Jump();
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        player.isJumping = false;
        player.anim.SetBool("Jumping", false);
    }

    public override void CheckStateUpdate() {
        if(player.characterController.isGrounded) {
            Debug.Log("doodoo");
            SwitchState(player.IdleState);
        }

        else if(player.playerManager.currentMovement.y < 0.0f) {
            SwitchState(player.FallingState);
        }

        else if(player.jumpsLeft > 0 && player.canJump) {
            SwitchState(player.JumpingState);
        }
    }

    private void Jump() {
        player.playerManager.currentMovement.y = player.initialJumpVelocity;
        // Weird band-aid to make Jump not randomly not work for some reason
        player.characterController.Move(player.playerManager.currentMovement * Time.deltaTime);
        player.jumpsLeft--;
        player.canJump = false;
    }
}
