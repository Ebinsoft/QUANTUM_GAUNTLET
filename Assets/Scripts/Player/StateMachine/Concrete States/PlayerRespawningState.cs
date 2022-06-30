using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawningState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerRespawningState(PlayerManager psm) : base(psm)
    {
        player = psm;

    }

    public override void EnterState()
    {
        player.isRespawning = true;
        // player.characterController.Move(new Vector3(0.0f, 5.0f, 0.0f));
        // TODO: Add in proper respawning once we have spawn points
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(0f, 5f, 0f);
        player.GetComponent<CharacterController>().enabled = true;


        player.stats.resetStats();
        // do animation stuff
        player.anim.Play("Idle");
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isRespawning = false;
        player.canDie = true;
    }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InRespawn"))
        {
            SwitchState(player.IdleState);
        }
    }
}
