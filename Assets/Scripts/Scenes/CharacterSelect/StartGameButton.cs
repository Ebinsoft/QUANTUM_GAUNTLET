using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour, IBasicButton
{
    Animator anim;
    private bool isFightVisible = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        var igr = IsGameReady();
        anim.SetBool("ReadyToStart", igr);

        if (anim.GetBool("ReadyToStart") && !isFightVisible)
        {
            Sound s = AudioManager.UISounds[UISound.CSFightReady];
            AudioManager.PlayAt(s, gameObject);
            isFightVisible = true;
        }

        if (!igr)
        {
            isFightVisible = false;
        }
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

    void IBasicButton.HoverEnter() { }
    void IBasicButton.HoverExit() { }

    void IBasicButton.Click()
    {
        if (IsGameReady())
        {
            Sound s = AudioManager.UISounds[UISound.CSFightClick];
            AudioManager.PlayAt(s, gameObject);
            GameManager.instance.TransitionToScene("VersusScene");
        }
    }
}
