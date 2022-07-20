using UnityEngine;

public class MeteorKick : SpecialAttackBehavior
{


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
                break;

            case Phase.Recovery:
                break;

        }
    }

    public override void OnExit() { }

    public override void OnHit(Collider other) { }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                currentPhase = Phase.Active;
                break;
        }
    }
}