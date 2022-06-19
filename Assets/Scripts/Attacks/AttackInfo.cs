using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewAttack", menuName = "ScriptableObjects/AttackInfo", order = 1)]
public class AttackInfo : ScriptableObject
{
    public string attackName;
    public int damage;
    public float hitlagTime;
    public float stunTime;
    public float knockback;
    public float knockup;
    public float knockupTime;
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
        EditorGUILayout.LabelField("(" + hitlagFrames + " frames of hitlag)");

        // Stun Time
        obj.stunTime = EditorGUILayout.FloatField("Stun Time", obj.stunTime);

        // Knockback
        obj.knockback = EditorGUILayout.FloatField("Knockback Distance", obj.knockback);

        // Knockup
        obj.knockup = EditorGUILayout.FloatField("Knockup Distance", obj.knockup);

        // Knockup Time
        EditorGUI.BeginDisabledGroup(obj.knockup <= 0);
        obj.knockupTime = EditorGUILayout.FloatField("Knockup Time", obj.knockupTime);
        EditorGUI.EndDisabledGroup();
    }
}