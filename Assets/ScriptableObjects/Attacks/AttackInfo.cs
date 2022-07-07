using System;
using UnityEngine;
using UnityEditor;

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

[CustomEditor(typeof(AttackInfo))]
public class AttackInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttackInfo obj = target as AttackInfo;
        SerializedObject sObj = new SerializedObject(obj);

        // Attack Name
        EditorGUI.BeginDisabledGroup(true);
        obj.attackName = EditorGUILayout.TextField("Attack Name", obj.name);
        EditorGUI.EndDisabledGroup();

        // Damage
        obj.damage = EditorGUILayout.IntField("Damage", obj.damage);

        // Mana cost and gain
        obj.manaCost = EditorGUILayout.IntField("Mana Cost", obj.manaCost);
        obj.manaGain = EditorGUILayout.IntField("Mana Gain Per Hit", obj.manaGain);

        // Hitlag Time
        obj.hitlagTime = EditorGUILayout.FloatField("Hitlag Time", obj.hitlagTime);
        float hitlagFrames = obj.hitlagTime * 60;
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("(" + hitlagFrames + " frames of hitlag)");
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        // Knockback
        EditorGUILayout.LabelField("Knockback", EditorStyles.boldLabel);
        string angleArrow;
        switch (obj.knockbackAngle)
        {
            case 0:
                angleArrow = "↑";
                break;
            case 90:
                angleArrow = "→";
                break;
            default:
                angleArrow = "↗";
                break;
        }
        obj.knockbackAngle = EditorGUILayout.Slider("Knockback Angle " + angleArrow, obj.knockbackAngle, 0, 90);
        obj.knockbackMagnitude = EditorGUILayout.FloatField("Knockback Magnitude", obj.knockbackMagnitude);
        using (new EditorGUI.DisabledScope(true))
        {
            // round displayed value to three decimal places
            Vector2 roundedKnockback = new Vector2(
                Mathf.Round(obj.knockback.x * 1000) / 1000,
                Mathf.Round(obj.knockback.y * 1000) / 1000);
            EditorGUILayout.Vector2Field("Knockback", roundedKnockback);
        }

        EditorGUILayout.Space();

        // Stun
        EditorGUILayout.LabelField("Stun", EditorStyles.boldLabel);
        obj.stunCalculation = (StunCalculation)EditorGUILayout.EnumPopup("Stun Calculation", obj.stunCalculation);

        if (obj.stunCalculation != StunCalculation.knockbackOnly)
        {
            obj.baseStun = EditorGUILayout.FloatField("Base Stun", obj.baseStun);
        }

        using (new EditorGUI.DisabledScope(true))
        {
            if (obj.stunCalculation != StunCalculation.baseOnly)
            {
                EditorGUILayout.FloatField("Knockback Stun", obj.knockbackStun);
            }
            EditorGUILayout.FloatField("Total Stun", obj.stunTime);
        }

        EditorGUILayout.Space();

        // Aim assist
        EditorGUILayout.LabelField("Aim Assist", EditorStyles.boldLabel);
        obj.hasAimAssist = EditorGUILayout.Toggle("Aim Assist", obj.hasAimAssist);
        if (obj.hasAimAssist) {
            EditorGUILayout.LabelField("Projects a cone out from the player's forward direction");
            EditorGUI.indentLevel++;
            obj.aimAssistAngle = EditorGUILayout.FloatField("Angle", obj.aimAssistAngle);
            obj.aimAssistDistance = EditorGUILayout.FloatField("Distance", obj.aimAssistDistance);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // Special Behavior
        EditorGUILayout.LabelField("Special Attack Behavior", EditorStyles.boldLabel);
        obj.hasSpecialBehavior = EditorGUILayout.Toggle("Is Special Attack", obj.hasSpecialBehavior);
        if (obj.hasSpecialBehavior)
        {
            EditorGUI.indentLevel++;
            obj.specialBehavior = SubclassSelector.Dropdown<SpecialAttackBehavior>("Special Behavior", obj.specialBehavior);
            EditorGUI.indentLevel--;
        }
        else
        {
            obj.specialBehavior = null;
        }

        EditorGUILayout.Space();

        // Sound Effects
        EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);

        obj.impactSoundType = (SoundEffectType)EditorGUILayout.EnumPopup("Impact Sound", obj.impactSoundType);
        EditorGUI.indentLevel++;
        switch (obj.impactSoundType)
        {
            case SoundEffectType.none:
                obj.customImpactSound = null;
                break;

            case SoundEffectType.preset:
                obj.presetImpactSound = (ImpactSound)EditorGUILayout.EnumPopup(obj.presetImpactSound);
                obj.customImpactSound = null;
                break;

            case SoundEffectType.custom:
                SerializedProperty prop = sObj.FindProperty("customImpactSound");
                EditorGUILayout.PropertyField(prop);
                break;

        }
        EditorGUI.indentLevel--;

        sObj.ApplyModifiedProperties();
        EditorUtility.SetDirty(obj);
    }
}