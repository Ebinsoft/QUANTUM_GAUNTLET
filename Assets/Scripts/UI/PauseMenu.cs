using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPane;
    private GameObject firstSelectedButton;
    private VolumeSlider musicSlider;
    private VolumeSlider sfxSlider;
    private bool isEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        isEnabled = false;
        pauseMenuPane = transform.Find("Pane").gameObject;
        firstSelectedButton = pauseMenuPane.transform.Find("Buttons").Find("Restart").gameObject;
        var buttons = pauseMenuPane.transform.Find("Buttons");
        musicSlider = buttons.Find("Music Volume").GetComponent<VolumeSlider>();
        sfxSlider = buttons.Find("SFX Volume").GetComponent<VolumeSlider>();
        DisablePauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleMenu()
    {
        if (!isEnabled)
        {
            EnablePauseMenu();
        }
        else
        {
            DisablePauseMenu();
        }
    }

    public void UpdateSliderVolumes()
    {
        musicSlider.UpdateSlider();
        sfxSlider.UpdateSlider();
    }

    public void EnablePauseMenu()
    {
        AudioManager.instance.masterMixer.SetFloat("MusicLowPassFreq", 500f);
        isEnabled = true;
        Time.timeScale = 0f;
        pauseMenuPane.SetActive(true);

        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set starting button
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        UpdateSliderVolumes();
    }

    public void DisablePauseMenu()
    {
        AudioManager.instance.masterMixer.SetFloat("MusicLowPassFreq", 22000f);
        isEnabled = false;
        Time.timeScale = 1f;
        pauseMenuPane.SetActive(false);
    }
}
