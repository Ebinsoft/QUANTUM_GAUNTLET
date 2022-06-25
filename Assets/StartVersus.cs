using UnityEngine;
using UnityEngine.SceneManagement;

public class StartVersus : MonoBehaviour
{
    public void StartVersusMatch()
    {
        SceneManager.LoadScene("VersusScene");
    }
}
