using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHitHandler : MonoBehaviour, IHitHandler
{
    public bool handleHit(HitData hitData)
    {
        AttackInfo attack = hitData.attack;




        return true;
    }
}
