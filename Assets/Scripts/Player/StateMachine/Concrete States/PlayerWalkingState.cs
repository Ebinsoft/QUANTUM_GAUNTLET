using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerMovingState(PlayerStateManager psm) : base(psm) {
        player = psm;
    }
    public override void EnterState() {
        player.isMoving = true;
        player.anim.SetBool("IsMoving", true);
    }

    public override void UpdateState() {
        Move();
    }

    public override void ExitState() {
        player.isMoving = false;
        player.playerManager.currentMovement.x = 0.0f;
        player.playerManager.currentMovement.z = 0.0f;
    }

    public override void CheckStateUpdate() {
        if(!player.isMovePressed) {
            SwitchState(player.IdleState);
        }

        if (player.isJumpPressed) {
            SwitchState(player.JumpingState);
        }

    
    }

    private void Move() {
        player.playerManager.currentMovement.x = player.playerSpeed * player.inputMovement.x;
        player.playerManager.currentMovement.z = player.playerSpeed * player.inputMovement.y;

        player.anim.SetFloat("WalkToRun", player.inputMovement.magnitude);
    }
}
