using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class CheckVersusReady : MonoBehaviour
{
    private GameObject startGameBox;
    private GameObject startGameText;
    void Start()
    {
        startGameBox = transform.Find("StartGameBox").gameObject;
        startGameText = transform.Find("StartGameText").gameObject;
    }
    void Update()
    {
        if (!startGameBox.activeInHierarchy && IsGameReady())
        {
            startGameBox.SetActive(true);
            startGameText.SetActive(true);
        }
        else if (startGameBox.activeInHierarchy && !IsGameReady())
        {
            startGameBox.SetActive(false);
            startGameText.SetActive(false);
        }
    }

    // This function will check the variety of conditions that
    // will dictate a playable game in order to enable the start button.
    private bool IsGameReady()
    {
        VersusInfo vi = GameManager.instance.versusInfo;
        List<PlayerSetting> playerSettings = vi.playerSettings;
        if (vi.numPlayers >= 2)
        {
            int uniqueTeams = playerSettings.Select(c => c.team.teamName)
            .Distinct()
            .Count();
            Debug.Log("U: " + uniqueTeams);
            if (uniqueTeams >= 2)
            {
                // also need to check that everyone has a character selected
                foreach (PlayerSetting ps in playerSettings)
                {
                    if (string.IsNullOrEmpty(ps.characterName))
                    {
                        return false;
                    }
                }
                return true;

            }


        }
        return false;
    }
}
