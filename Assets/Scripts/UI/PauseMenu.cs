using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPane;
    private GameObject firstSelectedButton;
    private bool isEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuPane = transform.Find("Pane").gameObject;
        firstSelectedButton = pauseMenuPane.transform.Find("Buttons").Find("Restart").gameObject;
        isEnabled = false;
        DisableGameOver();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleMenu()
    {
        if (!isEnabled)
        {
            EnableGameOver();
        }
        else
        {
            DisableGameOver();
        }
    }

    public void EnableGameOver()
    {

        isEnabled = true;
        Time.timeScale = 0f;
        pauseMenuPane.SetActive(true);

        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set starting button
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void DisableGameOver()
    {
        isEnabled = false;
        Time.timeScale = 1f;
        pauseMenuPane.SetActive(false);
    }
}
