using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Options : MonoBehaviour
{
    public GameObject optionScreen;
    public GameObject pauseMenuPane;
    // Start is called before the first frame update
    private GameObject ofirstSelectedButton;
    private GameObject pMenufirstSelectedButton;
    void Start()
    {
        ofirstSelectedButton = optionScreen.transform.Find("Buttons").Find("ExitOptions").gameObject;
        pMenufirstSelectedButton = pauseMenuPane.transform.Find("Buttons").Find("Restart").gameObject;
    }

    public void OpenOptions()
    {
        optionScreen.SetActive(true);
        pauseMenuPane.SetActive(false);
        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set starting button
        EventSystem.current.SetSelectedGameObject(ofirstSelectedButton);
    }

    public void CloseOptions()
    {
        optionScreen.SetActive(false);
        pauseMenuPane.SetActive(true);
        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set starting button
        EventSystem.current.SetSelectedGameObject(pMenufirstSelectedButton);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
