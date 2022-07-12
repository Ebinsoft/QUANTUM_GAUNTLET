using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ResItem
{
    public int horizontal,vertical;
}

public class Options : MonoBehaviour
{
    public GameObject optionScreen;
    public GameObject pauseMenuPane;
    // Start is called before the first frame update
    private GameObject ofirstSelectedButton;
    private GameObject pMenufirstSelectedButton;
    public Toggle fullscreenTog, vsyncTog;
    public List<ResItem>    resolutions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    void Start()
    {
        ofirstSelectedButton = optionScreen.transform.Find("FullScreen").Find("FullScreenTog").gameObject;
        pMenufirstSelectedButton = pauseMenuPane.transform.Find("Buttons").Find("Restart").gameObject;
        fullscreenTog.isOn = Screen.fullScreen;
        if( QualitySettings.vSyncCount == 0 )
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
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

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenTog.isOn;
        if(vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Screen.SetResolution(resolutions[selectedResolution].horizontal,
                             resolutions[selectedResolution].vertical,
                             fullscreenTog.isOn
                            );
    }

    public void ResLeft()
    {
        Debug.Log("hit res left");
        selectedResolution--;
        if(selectedResolution<0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }
    public void ResRight()
    {
        Debug.Log("hit res right");
        selectedResolution++;
        if(selectedResolution > resolutions.Count-1)
        {
            selectedResolution = resolutions.Count-1;
        }
        UpdateResLabel();
    }
    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString()+
                               " x " +
                               resolutions[selectedResolution].vertical.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}


