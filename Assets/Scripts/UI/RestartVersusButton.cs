using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartVersusButton : MonoBehaviour
{
    public void RestartVersusMatch()
    {
        GameManager.instance.TransitionToScene("VersusScene");
    }
}
