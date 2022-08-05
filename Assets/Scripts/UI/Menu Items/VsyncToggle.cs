using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VsyncToggle : MonoBehaviour
{
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = QualitySettings.vSyncCount != 0;
    }

    public void SetVsync()
    {
        QualitySettings.vSyncCount = toggle.isOn ? 1 : 0;
    }
}
