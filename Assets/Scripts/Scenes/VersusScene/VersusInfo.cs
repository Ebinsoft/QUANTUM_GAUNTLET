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
    public string gameType = "FFA";
    public int numLives = 3;
    public List<PlayerSetting> playerSettings;

    public void ResetPlayerTeams()
    {
        foreach (PlayerSetting ps in playerSettings)
        {
            ps.team.teamName = "Team " + (ps.playerIndex + 1);
        }
    }

    public PlayerSetting GetPlayer(int playerIndex)
    {
        return playerSettings.First(c => c.playerIndex == playerIndex);
    }

    public void RemovePlayer(int playerIndex)
    {
        if (playerSettings.Select(c => c.playerIndex).Contains(playerIndex))
        {
            PlayerSetting ps = playerSettings.First(c => c.playerIndex == playerIndex);
            playerSettings.Remove(ps);
            numPlayers--;
        }

    }

    public void AddPlayer(PlayerSetting ps)
    {
        // if a previous playerIndex exists, remove it
        RemovePlayer(ps.playerIndex);
        playerSettings.Add(ps);
        numPlayers++;
    }
}

[System.Serializable]
public class PlayerSetting
{
    public string playerName = "";
    public string playerType;
    public int playerIndex;
    public InputDevice device;
    public string deviceString;
    public Team team;
    public string characterName;
}
