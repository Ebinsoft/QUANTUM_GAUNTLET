using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartScene()
    {
        Scene s = SceneManager.GetActiveScene();
        GameManager.instance.TransitionToScene(s.name);
    }
}
