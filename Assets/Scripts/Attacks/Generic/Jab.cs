using System;
using UnityEngine;

[Serializable]
public class Jab : Attack
{

    public override string attackName
    {
        get { return "Jab"; }
    }
    public override int damage
    {
        get { return 1; }
    }
    public override float hitlagTime
    {
        get { return 0.15f; }
    }
    public override float stunTime
    {
        get { return 0.1f; }
    }

    public Jab(GameObject player)
    {
        findColliders(player);
    }

    private void findColliders(GameObject p)
    {
        Collider c;
        c = p.transform.Find("Model/Edmond/Armature/Spine1/Spine2/Spine3/Shouler.L/UpperArm.L/Forearm.L/Hand.L/Hand.L.Hitbox").GetComponent<Collider>();
        colliders.Add(c);
        c = p.transform.Find("Model/Edmond/Armature/Spine1/Spine2/Spine3/Shouler.L/UpperArm.L/Forearm.L/Forearm.L.Hitbox").GetComponent<Collider>();
        colliders.Add(c);
    }

}