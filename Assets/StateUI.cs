using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviour
{
    public Text stateText;
    public PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        stateText.text = "NULL";
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = player.currentState.ToString();
    }
}
