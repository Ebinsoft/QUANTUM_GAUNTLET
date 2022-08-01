using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupMenu : MonoBehaviour, ICancelHandler, IMoveHandler
{
    public GameObject selectedOnEnter;
    public GameObject selectedOnExit;


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

    public void OnMove(AxisEventData eventData)
    {
        Debug.Log("I'm gay bro");
    }

    public void OpenMenu()
    {
        backdrop.enabled = true;
        buttonPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedOnEnter);
    }

    public void CloseMenu()
    {
        backdrop.enabled = false;
        buttonPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedOnExit);
    }
}
