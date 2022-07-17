using UnityEngine;

public class PlayerAirLightAttackState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerAirLightAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
        canRotate = false;
        cancelMomentum = false;
    }

    public override void EnterState()
    {
        player.isAttacking = true;

        TriggerHit();
    }

    public override void UpdateState() { }

    public override void ExitState()
    {
        player.isAttacking = false;
    }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.AirborneState);
        }

        else if (player.isGrounded)
        {
            SwitchState(player.LandingState);
        }
    }

    void TriggerHit()
    {
        player.isLightAttackTriggered = false;
        player.anim.SetTrigger("LightAttack");
    }
}
