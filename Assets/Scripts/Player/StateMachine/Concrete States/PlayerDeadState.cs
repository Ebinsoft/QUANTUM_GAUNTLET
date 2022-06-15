using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerDeadState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.triggerDead = false;
        player.isDead = true;
        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(false);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(false);
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
    }

    public override void CheckStateUpdate()
    {

        if (!player.anim.GetBool("InDeath") && player.stats.lives > 1)
        {

            player.stats.lives--;
            SwitchState(player.RespawnState);
        }
    }
}
