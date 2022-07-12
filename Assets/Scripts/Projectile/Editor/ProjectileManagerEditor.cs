using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProjectileManager))]
public class ProjectileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProjectileManager obj = target as ProjectileManager;

        // projectile behavior selector
        obj.behaviorType = SubclassSelector.Dropdown<ProjectileBehavior>("Behavior", obj.behaviorType);

        // attack info
        obj.attack = (AttackInfo)EditorGUILayout.ObjectField("Attack", obj.attack, typeof(AttackInfo), false);

        // speeds
        obj.movementSpeed = EditorGUILayout.FloatField("Movement Speed", obj.movementSpeed);
        obj.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", obj.rotationSpeed);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(obj);
        }
    }
}