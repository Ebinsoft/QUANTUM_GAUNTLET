using System;
using UnityEngine;

public abstract class Attack
{
    public abstract string attackName { get; }
    public abstract int damage { get; }
    public abstract float hitlagTime { get; }
    public abstract float stunTime { get; }
    public Collider[] colliders;
}
