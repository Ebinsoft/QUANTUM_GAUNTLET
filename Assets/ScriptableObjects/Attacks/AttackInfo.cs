using System;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewAttack", menuName = "ScriptableObjects/AttackInfo", order = 1)]
public class AttackInfo : ScriptableObject
{
    public string attackName;
    public int damage;
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


    public bool hasSpecialBehavior = false;
    public SerializedClass specialBehavior = null;
}

public enum StunCalculation
{
    combined,
    baseOnly,
    knockbackOnly,
}

public struct HitData
{
    public AttackInfo attack;
    public Transform origin;
}

[CustomEditor(typeof(AttackInfo))]
public class AttackInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttackInfo obj = target as AttackInfo;

        // Attack Name
        EditorGUI.BeginDisabledGroup(true);
        obj.attackName = EditorGUILayout.TextField("Attack Name", obj.name);
        EditorGUI.EndDisabledGroup();

        // Damage
        obj.damage = EditorGUILayout.IntField("Damage", obj.damage);

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

        // Special Behavior
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

        EditorUtility.SetDirty(obj);
    }
}