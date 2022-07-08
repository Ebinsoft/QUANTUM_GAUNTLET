using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIcon : MonoBehaviour
{
    public PlayerManager player;
    private TextMesh textMesh;
    private int playerIndex;
    private PlayerSetting playerSetting;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        playerIndex = player.GetComponent<PlayerInput>().playerIndex;
        playerSetting = GameManager.instance.versusInfo.GetPlayer(playerIndex);

    }
    void Start()
    {
        textMesh.text = (playerSetting.playerType == PlayerType.Human ? "P" : "CPU") + (playerIndex + 1);
        textMesh.color = playerSetting.team.teamColor;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position - Camera.main.transform.position, Vector3.up);
    }
}
