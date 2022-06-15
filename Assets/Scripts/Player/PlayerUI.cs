using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text stateText;
    public PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        stateText.text = "NULL";
    }
    string GetUIString()
    {
        string name = player.transform.name;
        string hp = player.stats.health.ToString();
        string state = player.currentState.ToString();

        return name + "\n" + hp + "\n" + state;
    }
    // Update is called once per frame
    void Update()
    {
        stateText.text = GetUIString();
    }
}
