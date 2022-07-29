using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "ScriptableObjects/AttackInfo", order = 1)]
public class AttackInfo : ScriptableObject
{
    public string attackName;
    public int damage;
    public int manaCost;
    public int manaGain;
    public float hitlagTime;

    // KNOCKBACK
    public float knockbackAngle;
    public float knockbackMagnitude;
    public Vector2 knockback
    {
        get
        {
            var vec = new Vector2();
            vec.x = knockbackMagnitude * Mathf.Sin(Mathf.Deg2Rad * knockbackAngle);
            vec.y = knockbackMagnitude * Mathf.Cos(Mathf.Deg2Rad * knockbackAngle);
            return vec;
        }
    }

    // STUN
    public StunCalculation stunCalculation;
    public float baseStun;
    const float knockbackStunMultiplier = 4f / 60;
    public float knockbackStun
    {
        get { return knockbackMagnitude * knockbackStunMultiplier; }
    }
    public float stunTime
    {
        get
        {
            switch (stunCalculation)
            {
                case StunCalculation.combined:
                    return baseStun + knockbackStun;

                case StunCalculation.baseOnly:
                    return baseStun;

                case StunCalculation.knockbackOnly:
                    return knockbackStun;

                default:
                    Debug.LogError("Invalid StunCalculation type.");
                    return 0;
            }
        }
    }

    // Aim assist
    public bool hasAimAssist = false;
    public float aimAssistDistance;
    public float aimAssistAngle;

    // Special attack behavior
    public bool hasSpecialBehavior = false;
    public SerializedClass specialBehavior = null;

    // Sound effects
    public SoundEffectType impactSoundType = SoundEffectType.none;
    public ImpactSound presetImpactSound;
    public Sound customImpactSound;
}

public enum StunCalculation
{
    combined,
    baseOnly,
    knockbackOnly,
}

public enum SoundEffectType
{
    none,
    preset,
    custom
}

public struct HitData
{
    public AttackInfo attack;

    private Vector3 _direction;
    public Vector3 direction
    {
        get { return _direction; }
        set
        {
            value.y = 0;
            _direction = value.normalized;
        }
    }
}
