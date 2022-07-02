using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerBaseStats baseStats;
    public int health;
    public float mana;
    public int lives = 3;

    public bool canTakeDamage { get; private set; }
    public bool canGiveRecoil { get; private set; }
    public bool canGetStunned { get; private set; }

    private ShaderEffects shaderEffects;

    public enum Status
    {
        normal,
        intangible,
        armored,
        invulnerable
    }

    private Status _currentStatus = Status.normal;
    public Status currentStatus
    {
        get { return _currentStatus; }
        set
        {
            switch (value)
            {
                case Status.normal:
                    canTakeDamage = true;
                    canGiveRecoil = true;
                    canGetStunned = true;
                    break;
                case Status.intangible:
                    canTakeDamage = false;
                    canGiveRecoil = false;
                    canGetStunned = false;
                    break;
                case Status.armored:
                    canTakeDamage = true;
                    canGiveRecoil = true;
                    canGetStunned = false;
                    break;
                case Status.invulnerable:
                    canTakeDamage = false;
                    canGiveRecoil = true;
                    canGetStunned = false;
                    break;
                default:
                    Debug.LogError("Incorrectly set stat.Status");
                    break;
            }
            _currentStatus = value;
            shaderEffects.ApplyStatusFlicker(_currentStatus);
        }
    }

    void Awake()
    {
        canTakeDamage = true;
        canGetStunned = true;
        canGiveRecoil = true;
        resetStats();

        shaderEffects = GetComponentInChildren<ShaderEffects>();
    }

    public void resetStats()
    {
        health = baseStats.baseHealth;
        mana = baseStats.baseMana;
    }

    public void DrainMana(float amount)
    {
        mana = Mathf.Clamp(mana - amount, 0, baseStats.baseMana);
    }

    public void RestoreMana(float amount)
    {
        mana = Mathf.Clamp(mana + amount, 0, baseStats.baseMana);
    }
}
