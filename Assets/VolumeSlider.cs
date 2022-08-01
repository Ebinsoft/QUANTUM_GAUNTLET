using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    public string mixerName;
    public AudioMixer mixer;
    private Slider volumeSlider;

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        UpdateSlider();
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat(mixerName, Mathf.Log10(value) * 20);
    }

    public void UpdateSlider()
    {
        float db;
        mixer.GetFloat(mixerName, out db);
        volumeSlider.value = Mathf.Pow(10, (db / 20));
    }
}
