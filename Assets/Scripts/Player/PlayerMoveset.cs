using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveset : MonoBehaviour
{

    public Dictionary<MoveType, List<AttackInfo>> attackMoves;

    public Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        PopulateMoveset();
    }

    private void PopulateMoveset()
    {
        // initialize dictionary of attacks
        attackMoves = new Dictionary<MoveType, List<AttackInfo>>();
        foreach (MoveType type in Enum.GetValues(typeof(MoveType)))
        {
            attackMoves[type] = new List<AttackInfo>();
        }

        // populate moveset based on which animator states trigger attacks
        var attackStates = anim.GetBehaviours<AttackActivatorState>();
        var attackStateMachines = anim.GetBehaviours<AttackActivatorStateMachine>();

        foreach (AttackActivatorState attackActivator in attackStates)
        {
            attackMoves[attackActivator.moveType].Add(attackActivator.attack);
        }

        foreach (AttackActivatorStateMachine attackActivator in attackStateMachines)
        {
            attackMoves[attackActivator.moveType].Add(attackActivator.attack);
        }
    }

    // accumulated mana cost for all attacks tagged as a particular move type
    // may need to change in the future if we have more complex special attacks
    public int TotalManaCost(MoveType moveType)
    {
        return attackMoves[moveType].Select(a => a.manaCost).Sum();
    }
}

public enum MoveType
{
    LightAttack,
    HeavyAttack,
    Special1,
    Special2,
    Special3
}