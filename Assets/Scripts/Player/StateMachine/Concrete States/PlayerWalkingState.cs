using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerWalkingState(PlayerStateManager psm) : base(psm) {
        player = psm;
        canMove = true;
    }
    public override void EnterState() {
        player.isMoving = true;
        player.anim.SetBool("IsMoving", true);
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        player.isMoving = false;
        player.anim.SetBool("IsMoving", false);
    }

    public override void CheckStateUpdate() {
        if(!player.isMovePressed) {
            SwitchState(player.IdleState);
        }

        if (player.isJumpPressed && player.canJump) {
            SwitchState(player.JumpingState);
        }

    }

}
