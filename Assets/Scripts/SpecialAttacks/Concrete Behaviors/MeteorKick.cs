using UnityEngine;

public class MeteorKick : SpecialAttackBehavior
{

    float activeSpeed = 20f;

    private enum Phase
    {
        Startup,
        Active,
        Recovery
    }
    private Phase currentPhase;

    public override void OnEnter()
    {
        currentPhase = Phase.Startup;
        player.DisableGravity();
    }

    public override void Update()
    {
        switch (currentPhase)
        {
            case Phase.Startup:
                break;

            case Phase.Active:
                if (player.isGrounded)
                {
                    currentPhase = Phase.Recovery;
                    player.EnableGravity();
                    player.currentMovement = Vector3.zero;
                }
                break;

            case Phase.Recovery:
                break;
        }
    }

    public override void OnExit()
    {
        player.EnableGravity();
    }

    public override void OnHit(Collider other) { }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                currentPhase = Phase.Active;
                SetTrajectory();
                break;
        }
    }

    private void SetTrajectory()
    {
        Vector3 dir = player.transform.forward;
        dir.y = -3;
        dir.Normalize();

        player.currentMovement = dir * activeSpeed;
    }
}