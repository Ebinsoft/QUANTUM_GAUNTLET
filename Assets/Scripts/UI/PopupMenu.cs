using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupMenu : MonoBehaviour, ICancelHandler
{
    public GameObject selectedOnEnter;
    public GameObject selectedOnExit;
    public bool pauseWhileActive = false;


    // internal parts
    Image backdrop;
    GameObject buttonPanel;


    void Start()
    {
        backdrop = GetComponent<Image>();
        buttonPanel = transform.Find("Panel").gameObject;

        backdrop.enabled = false;
        buttonPanel.SetActive(false);
    }

    public void OnCancel(BaseEventData eventData)
    {
        CloseMenu();
    }

    public void OpenMenu()
    {
        if (pauseWhileActive)
        {
            Time.timeScale = 0;
        }

        backdrop.enabled = true;
        buttonPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedOnEnter);
    }

    public void CloseMenu()
    {
        if (pauseWhileActive)
        {
            Time.timeScale = 1;
        }

        backdrop.enabled = false;
        buttonPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedOnExit);
    }

    public void ToggleMenu()
    {
        if (buttonPanel.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
}
