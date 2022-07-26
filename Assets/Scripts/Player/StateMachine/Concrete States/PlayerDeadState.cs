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
        player.triggerDead = false;
        player.isDead = true;
        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(false);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(false);
        player.transform.Find("PlayerIcon").gameObject.SetActive(false);
        // do animation stuff
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        player.isDead = false;
        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(true);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(true);
        player.transform.Find("PlayerIcon").gameObject.SetActive(true);
    }

    public override void CheckStateUpdate()
    {

        if (!player.anim.GetBool("InDeath"))
        {
            if (player.stats.lives > 0)
            {
                player.stats.lives--;
                player.stats.health = 0;
                player.stats.PlayerDie();
                if (player.stats.lives == 0)
                {
                    player.stats.PlayerLose();
                }
                else
                {
                    SwitchState(player.RespawnState);
                }
            }

        }
    }
}
