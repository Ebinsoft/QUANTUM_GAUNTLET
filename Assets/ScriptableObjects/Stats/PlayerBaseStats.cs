using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "ScriptableObjects/Player/BaseStats", order = 1)]
public class PlayerBaseStats : ScriptableObject
{
    public int baseHealth;
    public int baseMana;
}