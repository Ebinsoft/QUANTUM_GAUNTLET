using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : MonoBehaviour
{
    public void LoadCharacterSelect()
    {
        //TODO remove this and fix the real problem - bandaid for frozen CS
        Time.timeScale = 1f;
        SceneManager.LoadScene("CharacterSelect");
    }
}
