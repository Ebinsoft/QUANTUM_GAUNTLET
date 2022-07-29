using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerBaseStats baseStats;
    public int health;
    public float mana;
    public int lives;

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
        ResetStats();

        shaderEffects = GetComponentInChildren<ShaderEffects>();
    }

    void Start()
    {
        PlayerSpawn();
    }

    void Update()
    {
        RestoreMana(baseStats.manaRegen * Time.deltaTime);
    }

    public void ResetStats()
    {
        health = baseStats.baseHealth;
        mana = baseStats.baseMana;
    }

    public void DrainHealth(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, baseStats.baseHealth);
    }

    public void RestoreHealth(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, baseStats.baseHealth);
    }

    public void DrainMana(float amount)
    {
        mana = Mathf.Clamp(mana - amount, 0, baseStats.baseMana);
    }

    public void RestoreMana(float amount)
    {
        mana = Mathf.Clamp(mana + amount, 0, baseStats.baseMana);
    }

    public event Action<GameObject> onPlayerSpawn;
    public void PlayerSpawn()
    {
        if (onPlayerSpawn != null)
        {
            onPlayerSpawn(gameObject);
        }
    }

    public event Action<GameObject> onPlayerLose;
    public void PlayerLose()
    {
        if (onPlayerLose != null)
        {
            onPlayerLose(gameObject);
        }
    }

    public event Action<GameObject> onPlayerDie;
    public void PlayerDie()
    {
        if (onPlayerDie != null)
        {
            onPlayerDie(gameObject);
        }
    }

    public event Action<GameObject> onPlayerRespawn;
    public void PlayerRespawn()
    {
        if (onPlayerRespawn != null)
        {
            onPlayerRespawn(gameObject);
        }
    }

    public event Action<GameObject> onPlayerDespawn;
    public void PlayerDespawn()
    {
        if (onPlayerDespawn != null)
        {
            onPlayerDespawn(gameObject);
        }
    }
}
