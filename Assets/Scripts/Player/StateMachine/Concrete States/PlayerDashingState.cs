using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    public PlayerManager player;
    private PlayerParticleEffects effects;
    private float dashTimer = 0.0f;
    private float dashDuration = 0.15f;
    private float dashSpeed;    // calculated from dashDuration and dashLength
    private float decayDuration = 0.15f;
    public PlayerDashingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;

        effects = player.gameObject.GetComponent<PlayerParticleEffects>();

        // calculate dashing speed
        dashSpeed = player.dashLength / dashDuration;
    }

    public override void EnterState()
    {
        player.isDashing = true;
        Dash();
    }

    public override void UpdateState()
    {
        dashTimer += Time.deltaTime;
        if (dashTimer >= dashDuration)
        {
            player.anim.SetBool("IsDashing", false);
            DecayVelocity();

            if (player.dashesLeft > 0 && player.isUtilityAttackTriggered)
            {
                Dash();
            }
        }
    }

    public override void ExitState()
    {
        player.isDashing = false;
        player.anim.SetBool("IsDashing", false);
        effects.StopDashingEffect();

        player.dashesLeft = player.maxDashes;
    }

    public override void CheckStateUpdate()
    {
        if (dashTimer >= dashDuration + decayDuration
            && !player.anim.GetBool("InDashRecovery"))
        {
            if (!player.characterController.isGrounded)
            {
                SwitchState(player.FallingState);
            }
            else
            {
                SwitchState(player.IdleState);
            }

            player.dashesLeft = player.maxDashes;
        }
    }

    private void Dash()
    {
        player.anim.SetBool("IsDashing", true);
        effects.StartDashingEffect();
        dashTimer = 0.0f;

        Vector3 playerFacing = player.transform.forward;
        player.currentMovement.x = (playerFacing.x * dashSpeed);
        player.currentMovement.z = (playerFacing.z * dashSpeed);

        player.dashesLeft--;
        player.isUtilityAttackTriggered = false;
    }

    private void DecayVelocity()
    {
        float decayAmt = Time.deltaTime * (dashSpeed / decayDuration);

        float horizontalSpeed = Mathf.Sqrt(
            Mathf.Pow(player.currentMovement.x, 2) +
            Mathf.Pow(player.currentMovement.z, 2));

        float newSpeed = Mathf.Max(0, horizontalSpeed - decayAmt);

        player.currentMovement.x = player.transform.forward.x * newSpeed;
        player.currentMovement.z = player.transform.forward.z * newSpeed;
    }
}
