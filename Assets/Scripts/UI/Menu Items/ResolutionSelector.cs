using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSelector : MonoBehaviour
{
    Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();

        dropdown.AddOptions(Screen.resolutions.Select(r => r.ToString()).ToList());

        dropdown.value = dropdown.options.FindIndex(o => o.text == Screen.currentResolution.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
