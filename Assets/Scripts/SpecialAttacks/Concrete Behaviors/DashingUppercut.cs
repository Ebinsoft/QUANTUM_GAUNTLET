
using UnityEngine;

public class DashingUppercut : SpecialAttackBehavior
{

    private float timeBeforeWhiff = 0.5f;

    private float timer;
    private bool firstHitTriggered;


    private enum Phase
    {
        Searching,
        Whiff,
        Hit
    }
    private Phase currentPhase;

    public override void OnEnter()
    {
        timer = 0;
        firstHitTriggered = false;
        currentPhase = Phase.Searching;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.Searching:
                if (timer >= timeBeforeWhiff)
                {
                    currentPhase = Phase.Whiff;
                    player.anim.SetTrigger("UppercutWhiff");
                }
                break;

            case Phase.Whiff:
                break;

            case Phase.Hit:
                break;
        }
    }

    public override void OnExit() { }

    public override void OnHit(Collider other)
    {
        if (currentPhase == Phase.Searching)
        {
            currentPhase = Phase.Hit;
            player.anim.SetTrigger("UppercutHit");
        }
    }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                // start dashing
                break;

            default:
                Debug.LogError("Invalid actionID: " + actionID);
                break;
        }
    }
}