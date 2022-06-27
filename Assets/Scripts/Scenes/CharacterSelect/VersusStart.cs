using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersusStart : MonoBehaviour
{
    public void StartVersusMatch()
    {
        SceneManager.LoadScene("VersusScene");
    }
}
