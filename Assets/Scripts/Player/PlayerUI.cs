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
        nameText.text = player.gameObject.name;
        int playerIndex = player.GetComponent<PlayerInput>().playerIndex;
        Vector3 startingPosition = transform.position;
        // extremely lazy offset TODO: Make this better.
        startingPosition.x += (300 * playerIndex);
        transform.position = startingPosition;
    }
}
