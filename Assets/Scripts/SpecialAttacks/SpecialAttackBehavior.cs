using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAttackBehavior
{
    // called once after special attack activates
    public abstract void OnEnter();

    // called every frame
    public abstract void Update();

    // called once after special attack finishes
    public abstract void OnExit();

    // exposed so that animations can trigger frame-specific actions
    public abstract void TriggerAction(int actionID);
}
