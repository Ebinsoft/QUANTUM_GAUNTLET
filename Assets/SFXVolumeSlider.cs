using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXVolumeSlider : MonoBehaviour
{
    [SerializeField]
    public string mixerName;
    public AudioMixer mixer;

    public void SetVolume(float value)
    {
        mixer.SetFloat(mixerName, Mathf.Log10(value) * 20);
    }
}
