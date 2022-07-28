using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefeatState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerDefeatState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {

        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(false);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(false);
        player.transform.Find("PlayerIcon").gameObject.SetActive(false);
        player.anim.Play("Die");

        player.stats.PlayerLose();

        player.playerKnockback.StopKnockback();
    }

    public override void UpdateState() { }

    public override void ExitState()
    {
        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(true);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(true);
        player.transform.Find("PlayerIcon").gameObject.SetActive(true);
    }

    public override void CheckStateUpdate()
    {
        // eternal void you can't leave 
    }
}
