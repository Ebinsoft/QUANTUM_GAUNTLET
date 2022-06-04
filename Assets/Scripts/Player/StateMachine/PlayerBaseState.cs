using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager manager);
    public abstract void UpdateState(PlayerStateManager manager);
    public abstract void ExitState(PlayerStateManager manager);
}
