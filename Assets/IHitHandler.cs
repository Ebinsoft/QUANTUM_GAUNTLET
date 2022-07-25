using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitHandler
{
    public bool handleHit(HitData hitData);
}
