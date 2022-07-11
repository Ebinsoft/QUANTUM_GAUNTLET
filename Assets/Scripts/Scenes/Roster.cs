using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Roster
{
    public CharacterData[] roster;

    public CharacterData GetCharacter(Character ch)
    {

        foreach (var c in roster)
        {
            if (c.character == ch)
            {
                return c;
            }
        }
        Debug.LogError("No character with that name found in roster");
        return null;
    }
}

public enum Character
{
    None = -1,
    Edmond
}

[System.Serializable]
public class CharacterData
{
    public Character character;
    public GameObject characterPrefab;
    public Sprite portrait;
}