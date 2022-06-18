using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "ScriptableObjects/AttackInfo", order = 1)]
public class AttackInfo : ScriptableObject
{
    public string attackName;
    public int damage;
    public float hitlagTime;
    public float stunTime;
}