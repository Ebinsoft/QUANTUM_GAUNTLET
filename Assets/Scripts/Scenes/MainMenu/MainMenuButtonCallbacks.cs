using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonCallbacks : MonoBehaviour
{

    public void SceneTransition(string scene)
    {
        Sound clickSound = AudioManager.UISounds[UISound.MainClick];
        AudioManager.PlayAt(clickSound, gameObject);
        GameManager.instance.TransitionToScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
