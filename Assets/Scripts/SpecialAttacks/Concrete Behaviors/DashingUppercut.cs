
using UnityEngine;

public class DashingUppercut : SpecialAttackBehavior
{

    // maximum distance and time spent dashing before the attack whiffs
    private float dashMaxDistance = 4f;
    private float dashMaxTime = 0.3f;

    // distance moved while decelerating after whiff
    private float whiffTravelDistance = 0.5f;

    private float dashSpeed, decelerationRate, prevFrameSpeed;
    private float timer;

    // VFX stuff
    PlayerParticleEffects particleEffects;

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
        decelerationRate = Mathf.Pow(dashSpeed, 2) / (2 * whiffTravelDistance);

        InitializeVFX();
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
                    HideVFX();
                    player.anim.SetTrigger("UppercutWhiff");
                }
                break;

            case Phase.Whiff:
                // decelerate velocity
                float currFrameSpeed = Mathf.Sqrt(
                    Mathf.Pow(player.currentMovement.x, 2) +
                    Mathf.Pow(player.currentMovement.z, 2)
                );
                currFrameSpeed = Mathf.Max(0, currFrameSpeed - decelerationRate * Time.deltaTime);

                float verletSpeed = (currFrameSpeed + prevFrameSpeed) / 2;
                prevFrameSpeed = currFrameSpeed;

                player.currentMovement.x = player.transform.forward.x * verletSpeed;
                player.currentMovement.z = player.transform.forward.z * verletSpeed;

                break;

            case Phase.Hit:
                break;
        }
    }

    public override void OnExit()
    {
    }

    public override void OnHit(Collider other)
    {
        if (currentPhase == Phase.Dashing)
        {
            HideVFX();

            currentPhase = Phase.Hit;
            player.anim.SetTrigger("UppercutHit");

            // stop all dashing movement on hit confirm
            player.currentMovement.x = 0;
            player.currentMovement.z = 0;

            // turn player towards opponent so that second hit connects
            Vector3 opponentPosition = other.attachedRigidbody.gameObject.transform.position;
            Vector3 opponentDirection = opponentPosition - player.transform.position;
            player.rotationTarget.x = opponentDirection.x;
            player.rotationTarget.y = opponentDirection.z;
        }
    }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                StartDashing();
                ShowVFX();
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

    private void InitializeVFX()
    {
        particleEffects = player.GetComponent<PlayerParticleEffects>();
    }

    private void ShowVFX()
    {
        particleEffects.StartFireDashingEffect();
    }

    private void HideVFX()
    {
        particleEffects.StopFireDashingEffect();
    }
}