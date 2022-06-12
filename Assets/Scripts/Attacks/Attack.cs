using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public abstract string attackName { get; }
    public abstract int damage { get; }
    public abstract float hitlagTime { get; }
    public abstract float stunTime { get; }
    public List<Collider> colliders = new List<Collider>();
}
