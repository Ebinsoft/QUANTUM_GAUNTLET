using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public string teamName;
    public Color teamColor
    {
        get
        {
            if (teamColors == null)
            {
                teamColors = new Dictionary<string, Color>() {
                    {"Team 1", Color.red},
                    {"Team 2", Color.blue},
                    {"Team 3", Color.green},
                    {"Team 4", Color.yellow}
                };
            }
            
            return teamColors[teamName];
        }
    }

    private IDictionary<string, Color> teamColors = new Dictionary<string, Color>()
    {
        {"Team 1", Color.red},
        {"Team 2", Color.blue},
        {"Team 3", Color.green},
        {"Team 4", Color.yellow}
    };

    public Team(string name)
    {
        teamName = name;
    }
}
