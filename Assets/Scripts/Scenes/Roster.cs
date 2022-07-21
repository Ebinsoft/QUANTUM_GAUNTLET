using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Roster
{
    public CharacterData[] roster;
    public StageData[] stages;
    private static System.Random rng = new System.Random();

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

    public StageData GetStage(Stage st)
    {
     foreach (var s in stages)
    {
        if (s.stage == st)
        {
            return s;
         }
    }
    Debug.LogError("No stage with that name found in roster");
    return null;
    }

    public StageData GetRandomStage()
    {
        int r = Random.Range(0, stages.Length);
        return stages[r];
    }
}



public enum Character
{
    None = -1,
    Edmond
}

public enum Stage
{
    None = -1,
    Random,
    Colosseum,
    FutureDumpSite
}
[System.Serializable]
public class CharacterData
{
    public Character character;
    public GameObject characterPrefab;
    public Sprite portrait;
    public Sprite fullBody;
}

[System.Serializable]
public class StageData
{
    public Stage stage;
    public string stageName;
    public GameObject stagePrefab;
    // Could maybe keep portrait and/or other stuff for stage select later
    // public Sprite portrait;
}