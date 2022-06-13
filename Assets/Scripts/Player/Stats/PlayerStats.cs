using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerBaseState baseStats;
    public int health = 100;


    private Status _currentStatus;
    public bool canTakeDamage { get; private set; }
    public bool canGetHit { get; private set; }
    public bool canGetStunned { get; private set; }

    public enum Status
    {
        normal,
        intangible,
        armored,
        invulnerable
    }

    public Status currentStatus
    {
        get { return _currentStatus; }
        set
        {
            switch (value)
            {
                case Status.normal:
                    canTakeDamage = true;
                    canGetHit = true;
                    canGetStunned = true;
                    break;
                case Status.intangible:
                    canTakeDamage = false;
                    canGetHit = false;
                    canGetStunned = false;
                    break;
                case Status.armored:
                    canTakeDamage = true;
                    canGetHit = true;
                    canGetStunned = false;
                    break;
                case Status.invulnerable:
                    canTakeDamage = false;
                    canGetHit = true;
                    canGetStunned = false;
                    break;
                default:
                    Debug.LogError("Incorrectly set stat.Status");
                    break;
            }
        }

    }


}
