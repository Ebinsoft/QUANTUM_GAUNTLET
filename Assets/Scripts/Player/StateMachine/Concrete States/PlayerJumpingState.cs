using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerJumpingState(PlayerStateManager psm) : base(psm){
        player = psm;
    }
    public override void EnterState() {
        Debug.Log("jump state");
        Jump();
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        player.isJumping = false;
        player.anim.SetBool("Jumping", false);
    }

    public override void CheckStateUpdate() {

    }

    private void Jump() {
        if (player.jumpsLeft > 0 && player.isJumpPressed) {
            player.playerManager.currentMovement.y = player.initialJumpVelocity;
            player.jumpsLeft--;
            player.isJumping = true;
            player.anim.SetBool("Jumping", true);
        }
    }
}
