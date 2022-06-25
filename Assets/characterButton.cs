using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class characterButton : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        {
            button = GetComponent<Button>();
        }
    }

    public void AddCharacter(Button button)
    {
        
    }
}
