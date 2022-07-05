using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Roster
{
    public CharacterData[] roster;

    public CharacterData GetCharacter(string characterName)
    {

        foreach (var c in roster)
        {
            if (c.characterName == characterName)
            {
                return c;
            }
        }
        Debug.LogError("No character with that name found in roster");
        return null;
    }
}

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public GameObject characterPrefab;
}