using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public void LoadCharacterSelect()
    {
        //TODO remove this and fix the real problem - bandaid for frozen CS
        pauseMenu.DisableGameOver();
        SceneManager.LoadScene("CharacterSelect");
    }
}
