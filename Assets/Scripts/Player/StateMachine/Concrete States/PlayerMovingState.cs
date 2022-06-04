using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerMovingState(PlayerStateManager psm) : base(psm) {
        player = psm;
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
