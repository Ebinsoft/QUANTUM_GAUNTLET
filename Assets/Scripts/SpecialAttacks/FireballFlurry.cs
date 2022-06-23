using UnityEngine;

public class FireballFlurry : SpecialAttackBehavior
{
    public override void OnEnter()
    {
        Debug.Log("I'm fucking entering");
    }

    public override void Update()
    {
        Debug.Log("I'm fucking updating");
    }

    public override void OnExit()
    {
        Debug.Log("I'm fucking exiting");
    }

    public override void TriggerAction(int actionID)
    {
        // todo shoot fireball
        Debug.Log("Trigger Action: " + actionID);
    }
}