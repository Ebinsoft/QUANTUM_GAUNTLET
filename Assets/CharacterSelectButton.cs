using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : MonoBehaviour
{
    public void LoadCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
