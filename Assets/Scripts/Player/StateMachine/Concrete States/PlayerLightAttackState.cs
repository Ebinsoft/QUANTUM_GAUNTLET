using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttackState : PlayerBaseState
{

    private PlayerManager player;


    private enum BufferedAttack
    {
        Light,
        Heavy
    }
    private List<BufferedAttack> bufferedAttacks;

    public PlayerLightAttackState(PlayerManager psm) : base(psm)
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

        bufferedAttacks = new List<BufferedAttack>();

        // trigger initial light attack
        player.isLightAttackTriggered = false;
        player.lightAttacksLeft--;
        player.anim.SetTrigger("LightAttack");
    }

    public override void UpdateState()
    {
        // buffer a subsequent light attack
        if (player.lightAttacksLeft > 0 && player.isLightAttackTriggered)
        {
            player.isLightAttackTriggered = false;
            player.lightAttacksLeft--;
            bufferedAttacks.Add(BufferedAttack.Light);
        }
        // buffer a heavy attack
        else if (player.isHeavyAttackTriggered)
        {
            player.isHeavyAttackTriggered = false;
            bufferedAttacks.Add(BufferedAttack.Heavy);
        }

        // if a light attack is buffered, trigger it
        if (!player.anim.GetBool("LightAttack") && NextBufferedIs(BufferedAttack.Light))
        {
            bufferedAttacks.RemoveAt(0);
            player.anim.SetTrigger("LightAttack");     // triggers the start of an attack
        }
    }

    public override void ExitState()
    {
        player.isAttacking = false;
        player.lightAttacksLeft = player.maxLightAttackChain;
    }

    public override void CheckStateUpdate()
    {
        // InMelee stays true until animator leaves the melee sub-state machine
        if (!player.anim.GetBool("InMelee"))
        {
            SwitchState(player.IdleState);
        }

        // if a heavy attack is buffered, transition to heavy atttack state
        else if (!player.anim.GetBool("LightAttack") && NextBufferedIs(BufferedAttack.Heavy))
        {
            SwitchState(player.HeavyAttackState);
        }
    }


    bool NextBufferedIs(BufferedAttack attackType)
    {
        return bufferedAttacks.Count > 0 && bufferedAttacks[0] == attackType;
    }
}
