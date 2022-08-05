using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerIcon : MonoBehaviour
{
    public PlayerManager player;
    private TextMeshPro textMesh;
    private PlayerSetting playerSetting;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        playerSetting = GameManager.instance.versusInfo.GetPlayer(player.playerID);

    }
    void Start()
    {
        textMesh.text = (playerSetting.playerType == PlayerType.Human ? "P" : "CPU") + (playerSetting.playerID + 1);
        textMesh.color = playerSetting.team.teamColor + Color.white * 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        // align sprite with camera
        transform.forward = Camera.main.transform.forward;

        // keep sprite from getting too small when camera is zoomed out
        float scale = Mathf.Clamp(Camera.main.orthographicSize / 3.5f, 1, 3);
        transform.localScale = Vector3.one * scale;
    }
}
