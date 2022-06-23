using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior
{
    public ProjectileManager projectile;

    // called once when the projectile is created
    public abstract void OnSpawn();

    // called upon collision with a new player
    public abstract void OnCollision();

    // called right before the projectile game object is destroyed
    public abstract void OnDestroy();

    // called every frame
    public abstract void Update();
}
