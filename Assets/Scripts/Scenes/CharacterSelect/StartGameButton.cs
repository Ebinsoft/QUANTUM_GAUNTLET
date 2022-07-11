using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("ReadyToStart", IsGameReady());
    }

    // This function will check the variety of conditions that
    // will dictate a playable game in order to enable the start button.
    private bool IsGameReady()
    {
        VersusInfo vi = GameManager.instance.versusInfo;
        PlayerSetting[] playerSettings = vi.playerSettings;
        if (vi.numPlayers >= 2)
        {
            IEnumerable<PlayerSetting> activePlayers = vi.GetActivePlayers();
            int uniqueTeams = activePlayers
                .Select(c => c.team.teamName)
                .Distinct()
                .Count();

            if (uniqueTeams >= 2)
            {
                // also need to check that everyone has a character selected
                foreach (PlayerSetting ps in activePlayers)
                {
                    if (ps.character == Character.None)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
    }

    public void Click()
    {
        Debug.Log("Poop");
        if (IsGameReady())
        {
            Debug.Log("GIGGA POOP");
            SceneManager.LoadScene("VersusScene");
        }
    }
}
