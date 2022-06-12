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
        Collider c = player.transform.Find("Model/Edmond/Armature/Spine1/Spine2/Spine3/Shoulder.L/UpperArm.L/Forearm.L/Hand.L/Hand.L.Hitbox").GetComponent<Collider>();

        colliders = new Collider[] { c };
    }

}