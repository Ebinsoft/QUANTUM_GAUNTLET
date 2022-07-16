using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
[System.Serializable]
public class VersusInfo
{
    public int numPlayers;
    public string stage;
    public GameMode gameType = GameMode.FFA;
    public int numLives = 3;
    public PlayerSetting[] playerSettings;


    public void ResetPlayers()
    {
        foreach (PlayerSetting ps in GetActivePlayers())
        {
            ps.playerType = PlayerType.None;
        }
    }
    public void ResetPlayerTeams()
    {
        foreach (PlayerSetting ps in GetActivePlayers())
        {
            ps.team.teamID = (TeamID)ps.playerID;
        }
    }

    public IEnumerable<PlayerSetting> GetActivePlayers()
    {
        return playerSettings.Where(c => c != null && c.playerType != PlayerType.None).Select(c => c);
    }

    public PlayerSetting GetPlayer(int playerID)
    {
        return playerSettings[playerID];
    }

    public void RemovePlayer(int playerID)
    {
        PlayerSetting ps = GetPlayer(playerID);
        if (ps != null && ps.playerType != PlayerType.None)
        {
            ps.playerName = "";
            ps.playerType = PlayerType.None;
            ps.device = null;
            ps.deviceString = "";
            ps.team.teamID = (TeamID) ps.playerID;
            ps.character = Character.None;
            numPlayers--;
        }
    }


    public void AddPlayer(PlayerSetting ps)
    {
        // if a previous playerID exists, remove it
        RemovePlayer(ps.playerID);
        // playerSettings.Add(ps);
        playerSettings[ps.playerID] = ps;
        numPlayers++;
    }
}

public enum GameMode 
{
    FFA,
    Team
}

[System.Serializable]
public class PlayerSetting
{
    public int playerID;
    public string playerName = "";
    public PlayerType playerType;
    public InputDevice device;
    public string deviceString;
    public Team team;
    public Character character;
}
public enum PlayerType
{
    None,
    Human,
    Robot
}
