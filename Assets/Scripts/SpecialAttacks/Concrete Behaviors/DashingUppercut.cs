
using UnityEngine;

public class DashingUppercut : SpecialAttackBehavior
{

    // maximum distance and time spent dashing before the attack whiffs
    private float dashMaxDistance = 3f;
    private float dashMaxTime = 0.2f;

    // distance and time while decelerating after whiff
    private float whiffTravelDistance = 0.3f;
    private float whiffTravelTime = 0.3f;

    private float dashSpeed, prevFrameSpeed;
    private float timer;

    private enum Phase
    {
        Dashing,
        Whiff,
        Hit
    }
    private Phase currentPhase;

    public override void OnEnter()
    {
        timer = 0;
        currentPhase = Phase.Dashing;

        dashSpeed = dashMaxDistance / dashMaxTime;
        prevFrameSpeed = dashSpeed;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.Dashing:
                if (timer >= dashMaxTime)
                {
                    currentPhase = Phase.Whiff;
                    player.anim.SetTrigger("UppercutWhiff");
                }
                break;

            case Phase.Whiff:
                float timeDecelerating = timer - dashMaxTime;

                float currentSpeed = Mathf.Clamp((1 - timeDecelerating / whiffTravelTime), 0, 1) * dashSpeed;
                float verletSpeed = (currentSpeed + prevFrameSpeed) / 2;

                player.currentMovement.x = verletSpeed * player.transform.forward.x;
                player.currentMovement.z = verletSpeed * player.transform.forward.z;

                prevFrameSpeed = currentSpeed;
                break;

            case Phase.Hit:
                break;
        }
    }

    public override void OnExit() { }

    public override void OnHit(Collider other)
    {
        if (currentPhase == Phase.Dashing)
        {
            currentPhase = Phase.Hit;
            player.anim.SetTrigger("UppercutHit");

            // stop all dashing movement on hit confirm
            player.currentMovement.x = 0;
            player.currentMovement.z = 0;
        }
    }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                StartDashing();
                break;

            default:
                Debug.LogError("Invalid actionID: " + actionID);
                break;
        }
    }

    private void StartDashing()
    {
        Vector3 velocity = player.transform.forward * dashSpeed;
        player.currentMovement.x = velocity.x;
        player.currentMovement.z = velocity.z;

        timer = 0;
    }
}