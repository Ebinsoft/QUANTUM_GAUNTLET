using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerIdleState(PlayerStateManager psm) : base(psm) {
        player = psm;
    }
    public override void EnterState() {
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        
    }

    public override void CheckStateUpdate() {
        if(player.isJumpPressed) {
            SwitchState(player.JumpingState);
        }

        if(player.isMovePressed) {
            SwitchState(player.MovingState);
        }
    }
}