using UnityEngine;

public class PlayerAirHeavyAttackState : PlayerBaseState
{

    private PlayerManager player;

    public PlayerAirHeavyAttackState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.isAttacking = true;
        player.anim.SetBool("InMelee", true);


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
        player.anim.SetTrigger("HeavyAttack");
    }
}
