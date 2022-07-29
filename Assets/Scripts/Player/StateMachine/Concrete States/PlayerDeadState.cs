using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerDeadState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;

    }

    public override void EnterState()
    {
        player.isDead = true;


        player.stats.lives--;
        player.stats.health = 0;
        player.stats.PlayerDie();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        player.isDead = false;
        player.stats.PlayerDespawn();
        // explode stuff should happen here
    }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InDying"))
        {
            if (player.stats.lives == 0)
            {
                SwitchState(player.DefeatState);
            }
            else
            {
                SwitchState(player.RespawnState);
            }
        }
    }
}
