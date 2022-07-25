using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonCallbacks : MonoBehaviour
{
    public void SceneTransition(string scene)
    {
        GameManager.instance.TransitionToScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
