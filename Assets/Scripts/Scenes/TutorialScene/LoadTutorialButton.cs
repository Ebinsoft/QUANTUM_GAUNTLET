using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTutorialButton : MonoBehaviour
{
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}
