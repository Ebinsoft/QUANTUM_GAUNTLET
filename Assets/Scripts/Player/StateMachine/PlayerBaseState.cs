using UnityEngine;

public abstract class PlayerBaseState
{
    private PlayerStateManager player;
    public PlayerBaseState(PlayerStateManager psm) {
        player = psm;
        
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckStateUpdate();

    public void Update() {
        CheckStateUpdate();
        UpdateState();
    }

    public void SwitchState(PlayerBaseState newState) {
        player.currentState.ExitState();
        player.currentState = newState;
        newState.EnterState();
    }
}
