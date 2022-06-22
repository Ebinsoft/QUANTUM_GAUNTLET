using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roster : MonoBehaviour
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
