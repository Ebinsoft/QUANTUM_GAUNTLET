using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]
public class VersusInfo
{
    public int numPlayers;
    public string stage;
    public string gameType = "FFA";
    public int numLives = 3;
    public List<PlayerSettings> playerSettings;
}

[System.Serializable]
public class PlayerSettings
{
    public string playerName = "";
    public string playerType;
    public int playerIndex;
    public InputDevice device;
    public string deviceString;
    public string team;
    public string characterName;
}
