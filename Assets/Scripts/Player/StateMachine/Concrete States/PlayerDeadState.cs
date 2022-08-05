using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private PlayerManager player;

    float timer;
    float timeBeforeExplode = 0.5f;
    float timeAfterExplode = 0.75f;
    bool exploded;

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

        timer = 0;
        exploded = false;

        player.stats.lives--;
        player.stats.health = 0;
        player.stats.PlayerDie();

    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if ((timer >= timeBeforeExplode || !player.delayBeforeDeathExplosion) && !exploded)
        {
            player.GetComponent<PlayerParticleEffects>().PlayDeathExplosion();
            HidePlayer();

            exploded = true;
            timer = 0;
        }
    }

    public override void ExitState()
    {
        player.isDead = false;
        player.stats.PlayerDespawn();
        ShowPlayer();
    }

    public override void CheckStateUpdate()
    {
        if (exploded && timer >= timeAfterExplode)
        {
            if (player.stats.lives <= 0)
            {
                SwitchState(player.DefeatState);
            }
            else
            {
                SwitchState(player.RespawnState);
            }
        }
    }

    void HidePlayer()
    {
        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(false);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(false);
        player.transform.Find("PlayerIcon").gameObject.SetActive(false);
    }

    void ShowPlayer()
    {
        player.transform.Find("Model/Edmond/Armature").gameObject.SetActive(true);
        player.transform.Find("Model/Edmond/Body").gameObject.SetActive(true);
        player.transform.Find("PlayerIcon").gameObject.SetActive(true);
    }
}
