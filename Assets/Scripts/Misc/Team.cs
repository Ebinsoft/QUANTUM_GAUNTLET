using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public TeamID teamID;
    public string teamName
    {
        get
        {
            return teamInfo[teamID].Item1;
        }
    }

    public Color teamColor
    {
        get
        {
            return teamInfo[teamID].Item2;
        }
    }

    private static IDictionary<TeamID, (string, Color)> teamInfo = null;

    private static void PopulateDictionary()
    {
        teamInfo = new Dictionary<TeamID, (string, Color)>();
        teamInfo.Add(TeamID.Team1, ("Red", new Color(0.6941177f, 0.2431373f, 0.3254902f)));
        teamInfo.Add(TeamID.Team2, ("Blue", new Color(0.2313726f, 0.3647059f, 0.7882353f)));
        teamInfo.Add(TeamID.Team3, ("Green", new Color(0.2196078f, 0.7176471f, 0.3921569f)));
        teamInfo.Add(TeamID.Team4, ("Yellow", new Color(1f, 0.7180066f, 0.2216981f)));
    }

    static Team()
    {
        if (teamInfo == null)
        {
            PopulateDictionary();
        }
    }

    public Team(TeamID tID)
    {
        teamID = tID;
    }
}

public enum TeamID
{
    Team1 = 0,
    Team2 = 1,
    Team3 = 2,
    Team4 = 3
}