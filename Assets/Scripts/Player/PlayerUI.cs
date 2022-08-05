using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    public Text nameText;
    public PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 startingPosition = transform.position;
        // extremely lazy offset TODO: Make this better.
        startingPosition.x += (300 * player.playerID);
        transform.position = startingPosition;
    }

    void Update()
    {
        nameText.text = player.currentState.ToString();
    }
}
