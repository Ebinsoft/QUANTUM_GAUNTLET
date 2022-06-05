using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerFallingState(PlayerStateManager psm) : base(psm) {
        player = psm;
        canMove = true;
    }
    public override void EnterState() {
        
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        
    }

    public override void CheckStateUpdate() {
        
    }
}
