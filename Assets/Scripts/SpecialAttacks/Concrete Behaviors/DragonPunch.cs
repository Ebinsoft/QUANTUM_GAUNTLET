using System.Collections.Generic;
using UnityEngine;

public class DragonPunch : SpecialAttackBehavior
{
    bool buttonWasReleased;
    bool punchCanBeReleased;
    bool punchWasReleased;


    public override void OnEnter()
    {
        buttonWasReleased = false;
        punchCanBeReleased = false;
        punchWasReleased = false;
    }

    public override void Update()
    {
        if (!player.isSpecial2Pressed) buttonWasReleased = true;

        if (punchCanBeReleased &&
            !punchWasReleased &&
            buttonWasReleased)
        {
            player.anim.SetTrigger("ReleasePunch");
            punchWasReleased = true;
        }
    }

    public override void OnExit() { }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                // punch has reached its minimum charge time
                punchCanBeReleased = true;
                break;

            case 1:
                // punch has reached its maximum charge time, forced release
                punchWasReleased = true;
                break;
        }
    }
}